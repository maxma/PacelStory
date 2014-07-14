using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using PacelStory.Repositories;
using PacelStory.Models;
using System.Net.Http;
using System.Net;
using PacelStory.Utilities;

namespace PacelStory.Controllers
{
    [RoutePrefix("api/v1/Customer")]
    public class CustomerV1Controller : ApiController
    {
        //
        // GET: /Customer/
        CustomerRepository cr = new CustomerRepository();
        CustomerResponseString crs = new CustomerResponseString();


        // GET api/get 
        [Route("GetAllCustomers/{pageNumber}")]
        [HttpGet]
        //[ActionName("AllCustomers")]
        public List<Customer> GetAllCustomers(int pageNumber)
        {
            if (pageNumber < 1)
            {
                return null;
            }
            else
            {
                return cr.GetAllCustomers(pageNumber).ToList();
            }
        }

        [Route("SpecifiedCustomer/{customerId}")]
        [HttpGet]
        //[ActionName("SpecifiedCustomer")]
        public Customer GetSpecifiedCustomer(long customerId)
        {
            if (customerId < 1)
            {
                return null;
            }
            else
            {
                return cr.GetSpecifiedCustomerById(customerId);
            }
        }


        // POST api/values  业主版 输入 验证码 调用 此接口
        [Route("CheckCode")]
        [HttpPost]
        //[ActionName("CheckCode")]
        public HttpResponseMessage CheckValidationCode([FromBody]Customer customer)
        {
            try
            {
                if (customer.validationCode.Trim().Length >= 4)
                {

                    long effectedCustomerId = 0;

                    Customer temp = cr.GetSpecifiedCustomerByMoble(customer.mobile);


                    #region  专门为了提交苹果商店验证，看到用户版内容，添加的代码

                    if (temp.mobile.Trim() == "18610739780")
                    {
                        crs = CommonUtility.FormatCustomerResponseString(0, temp.customerId, "Succeed,verify code success");
                        return Request.CreateResponse(HttpStatusCode.Created, crs);
                    }
                    #endregion

                    if (temp.validationCodeTime != null)
                    {
                        if (temp.validationCode == customer.validationCode)
                        {
                            TimeSpan span = DateTime.Now.Subtract((DateTime)temp.validationCodeTime);
                            if (span.TotalSeconds <= 480)  // 判断验证码 是否过期
                            {
                                // 更新用户, 首先要保留原有用户数据，只更新validation code, 因为用户登陆时除了电话和验证码没有任何信息，不能全部更新。
                                customer = temp;
                                customer.validationCode = "2";
                                effectedCustomerId = cr.UpdateCustomer(customer);

                                if (effectedCustomerId == 0)
                                {
                                    crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "Failed,can not save to the db");
                                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                                }
                                else
                                {
                                    crs = CommonUtility.FormatCustomerResponseString(0, effectedCustomerId, "Succeed,verify code success");
                                    return Request.CreateResponse(HttpStatusCode.Created, crs);
                                }
                            }
                            else
                            {
                                crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "验证码过期.");
                                return Request.CreateResponse(HttpStatusCode.OK, crs);
                            }
                        }
                        else
                        {
                            crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "验证码不匹配.");
                            return Request.CreateResponse(HttpStatusCode.OK, crs);
                        }
                    }
                    else
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "Failed,validation code in temp from db is null.");
                        return Request.CreateResponse(HttpStatusCode.OK, crs);
                    }
                }
                else
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,customer json format is wrong or validation code is not correct format");
                    return Request.CreateResponse(HttpStatusCode.OK, crs);
                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }


        }


        // POST api/values   业主版 输入 手机号 调用此 接口
        [Route("CreateCustomer")]
        [HttpPost]
        //[ActionName("CreateCustomer")]
        public HttpResponseMessage CreateCustomer([FromBody]Customer customer)
        {
            try
            {
                long effectedCustomerId = 0;

                if (customer == null || customer.mobile.Trim() == "" || customer.mobile == null)
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    // 第一次 创建用户，并且创建验证码
                    //if (customer.validationCode == "0")
                    //{
                    // 产生验证码
                    customer.validationCode = CommonUtility.GenerateValidationCode();

                    #region send validationCode text
                    // 发验证码短信  
                    TextRepository tr = new TextRepository();
                    // struct textFormat
                    tbl_smsmt_send textFormat = new tbl_smsmt_send();
                    textFormat.account = "2521494";
                    textFormat.mobile = customer.mobile;

                    string productName = " 【指尖社区】";
                    textFormat.content = "您的验证码为 " + customer.validationCode + productName;
                    textFormat.smsid = "0567898f30e658dbff5a";
                    textFormat.priority = 1;
                    textFormat.SubmitTime = DateTime.Now;

                    int sendSuccess = tr.CreateText(textFormat);
                    #endregion

                    customer.validationCodeTime = DateTime.Now;
                    Customer temp = cr.GetSpecifiedCustomerByMoble(customer.mobile);
                    if (temp == null)
                    {
                        effectedCustomerId = cr.CreateCustomer(customer); // 新用户登录                            
                    }
                    else
                    {
                        // 用户之前就存在，重新登录， 用户基本信息还是用数据库里的，只是验证码和验证码时间 更新一下。
                        temp.validationCode = customer.validationCode;
                        //customer = temp;
                        temp.validationCodeTime = DateTime.Now;
                        effectedCustomerId = cr.UpdateCustomer(temp);
                    }

                    if (effectedCustomerId == 0)
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "Failed,can not save to the db");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                    }
                    else
                    {
                        crs = CommonUtility.FormatCustomerResponseString(0, effectedCustomerId, "Succeed,Create customer Success,validation code sent");
                        return Request.CreateResponse(HttpStatusCode.Created, crs);

                    }

                    //}
                    //// 验证码 验证通过 更新用户信息，并且重置 validationCode 为 2; 表示以前通过验证过。
                    //else
                    //{                        
                    //    Customer temp = cr.GetSpecifiedCustomerByMoble(customer.mobile);
                    //    if (temp.validationCodeTime != null)
                    //    {
                    //        if (temp.validationCode == customer.validationCode)
                    //        {
                    //            TimeSpan span = DateTime.Now.Subtract((DateTime)temp.validationCodeTime);
                    //            if (span.TotalSeconds <= 60)  // 判断验证码 是否过期
                    //            {
                    //                // 更新用户
                    //                customer.validationCode = "2";
                    //                effectedRows = cr.UpdateCustomer(customer);

                    //                if (effectedRows == 0)
                    //                {
                    //                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "can not save to the db.");
                    //                }
                    //                else
                    //                {
                    //                    return Request.CreateResponse(HttpStatusCode.Created, "updated items in db success.");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                return Request.CreateResponse(HttpStatusCode.OK, "验证码过期.");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return Request.CreateResponse(HttpStatusCode.OK, "验证码不匹配.");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "not standard request by validation code.");
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }

        }

        // PUT api/values
        [Route("UpdateCustomer")]
        [HttpPatch]
        [HttpPut]
        //[ActionName("UpdateCustomer")]
        public HttpResponseMessage UpdateCustomer([FromBody]Customer customer)
        {
            try
            {
                if (customer == null)  // no pacel to update
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    // try to retrieve pacel in db.
                    Customer dbCustomer = cr.GetSpecifiedCustomerById(customer.customerId);

                    // update a non-existed pacel
                    if (dbCustomer == null || customer.customerId != dbCustomer.customerId)
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,target object not found");
                        return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                    }
                    else
                    {

                        long effectedCustomerId = 0;
                        effectedCustomerId = cr.UpdateCustomer(customer);
                        if (effectedCustomerId == 0)
                        {
                            crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "Failed,can not save to the db");
                            return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                        }
                        else
                        {
                            crs = CommonUtility.FormatCustomerResponseString(0, effectedCustomerId, "Succeed,update items in db success");
                            return Request.CreateResponse(HttpStatusCode.OK, crs);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }

        }

        [Route("RemoveCustomer/{id}")]
        [HttpDelete]
        //[ActionName("RemoveCustomer")]
        public HttpResponseMessage RemoveCustomer(long id)
        {
            try
            {
                if(id < 1)
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,id<1");
                    return Request.CreateResponse(HttpStatusCode.NotFound, crs);
                }
                // try to retrieve pacel in db.
                var customer = cr.GetSpecifiedCustomerById(id);

                if (customer == null)  // no community to delete
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not find target you delete");
                    return Request.CreateResponse(HttpStatusCode.NotFound, crs);
                }
                else
                {

                    int effectedRows = 0;
                    effectedRows = cr.RemoveCustomer(customer.customerId);
                    if (effectedRows == 0)
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not save to the db");
                        return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                    }
                    else
                    {
                        crs = CommonUtility.FormatCustomerResponseString(0, 0, "Succeed,delete items in db success");
                        return Request.CreateResponse(HttpStatusCode.OK, crs);
                    }

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }
        }

        // POST api/values
        [Route("CreateCustomerType2")]
        [HttpPost]
        //[ActionName("CreateCustomerType2")]
        public HttpResponseMessage CreateCustomerType2([FromBody]Customer customer)
        {
            try
            {
                long effectedCustomerId = 0;

                if (customer == null || customer.mobile.Trim() == "" || customer.mobile == null)
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    // 第一次 创建用户，并且创建验证码
                    //if (customer.validationCode == "0")
                    //{
                    // 产生验证码
                    customer.validationCode = CommonUtility.GenerateValidationCode();

                    #region send validationCode text
                    // 发验证码短信  
                    TextRepository tr = new TextRepository();
                    // struct textFormat
                    tbl_smsmt_send textFormat = new tbl_smsmt_send();
                    textFormat.account = "2521494";
                    textFormat.mobile = customer.mobile;

                    string productName = " 【指尖社区】";
                    textFormat.content = "您的验证码为 " + customer.validationCode + productName;
                    textFormat.smsid = "0567898f30e658dbff5a";
                    textFormat.priority = 1;
                    textFormat.SubmitTime = DateTime.Now;

                    int sendSuccess = tr.CreateText(textFormat);
                    #endregion

                    customer.validationCodeTime = DateTime.Now;
                    Customer temp = cr.GetSpecifiedCustomerByMoble(customer.mobile);
                    if (temp == null)
                    {
                        effectedCustomerId = cr.CreateCustomer(customer); // 新用户登录                            
                    }
                    else
                    {
                        // 用户之前就存在，重新登录， 用户基本信息还是用数据库里的，只是验证码和验证码时间 更新一下。
                        temp.validationCode = customer.validationCode;
                        customer = temp;
                        customer.validationCodeTime = DateTime.Now;
                        effectedCustomerId = cr.UpdateCustomer(customer);
                    }

                    if (effectedCustomerId == 0)
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, effectedCustomerId, "Failed,can not save to the db");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                    }
                    else
                    {
                        crs = CommonUtility.FormatCustomerResponseString(0, effectedCustomerId, "Succeed,Create customer Success,validation code sent");
                        return Request.CreateResponse(HttpStatusCode.Created, crs);

                    }

                    //}
                    //// 验证码 验证通过 更新用户信息，并且重置 validationCode 为 2; 表示以前通过验证过。
                    //else
                    //{                        
                    //    Customer temp = cr.GetSpecifiedCustomerByMoble(customer.mobile);
                    //    if (temp.validationCodeTime != null)
                    //    {
                    //        if (temp.validationCode == customer.validationCode)
                    //        {
                    //            TimeSpan span = DateTime.Now.Subtract((DateTime)temp.validationCodeTime);
                    //            if (span.TotalSeconds <= 60)  // 判断验证码 是否过期
                    //            {
                    //                // 更新用户
                    //                customer.validationCode = "2";
                    //                effectedRows = cr.UpdateCustomer(customer);

                    //                if (effectedRows == 0)
                    //                {
                    //                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "can not save to the db.");
                    //                }
                    //                else
                    //                {
                    //                    return Request.CreateResponse(HttpStatusCode.Created, "updated items in db success.");
                    //                }
                    //            }
                    //            else
                    //            {
                    //                return Request.CreateResponse(HttpStatusCode.OK, "验证码过期.");
                    //            }
                    //        }
                    //        else
                    //        {
                    //            return Request.CreateResponse(HttpStatusCode.OK, "验证码不匹配.");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "not standard request by validation code.");
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }
        }
        

    }
}

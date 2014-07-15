using PacelStory.Models;
using PacelStory.Repositories;
using PacelStory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PacelStory.Controllers
{
    [RoutePrefix("api/v1/CampOwner")]
    public class CampOwnerV1Controller : ApiController
    {
        CampOwnerResponseString co_responseString = new CampOwnerResponseString();
        CommunityResponseString communityResponseString = new CommunityResponseString();

        CampOwnerRepository campOwnerRepository = new CampOwnerRepository();
        CommunityRepository communityRepositoy = new CommunityRepository();
        CustomerRepository customerRepository = new CustomerRepository();

        // POST api/values     H5 验证用户名密码 调用此接口

        [Route("CheckCampOwnerCode/{username}/{pwd}")]
        [HttpGet]
        //[ActionName("CheckCampOwnerCode")]
        
        public HttpResponseMessage CheckCampOwnerCode(string username, string pwd)
        {
            if (username.Trim() == "zhijian" && pwd.Trim() == "zhijian")
            {
                CampOwnerAndCommunity coac = new CampOwnerAndCommunity();
                co_responseString = CommonUtility.FormatCampOwnerResponseString(0, 0, "Succeed,validate user password success");
                return Request.CreateResponse(HttpStatusCode.OK, co_responseString);
            }
            else
            {
                co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, 0, "Failed,user password not match");
                return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
            }
        }

        // POST api/values   H5 关联 物业 小区 时调用此接口
        [Route("CreateCustomerIntoCommunity")]
        [HttpPost]
        //[ActionName("CreateCustomerIntoCommunity")]
        public HttpResponseMessage CreateCustomerIntoCommunity([FromBody]CampOwnerAndCommunity item)
        {
            try
            {
                if (item == null || item.campOwner == null || item.community == null || item.campStaffMobileList == null || item.campStaffMobileList.Count == 0)
                {                    
                    co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, 0, "Failed,can not read object from body or no mobile input");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
                }
                else
                {
                    // 将社区信息 赋给 物业人员，并且创建该物业人员
                    foreach (var mobile in item.campStaffMobileList)
                    {
                        Customer customer = new Customer();
                        customer.mobile = mobile;
                        customer.province = item.community.province;
                        customer.city = item.community.city;
                        customer.district = item.community.district;
                        customer.campname = item.community.campname;
                        customer.bldNumber = item.community.bldNumber;
                        customer.unitNumber = item.community.unitNumber;
                        customer.roomNumber = item.community.roomNumber;
                        customer.type = "1";
                        customer.communityId = item.community.communityId;
                        customer.campCode = item.community.campCode;

                        long effectedCustomerId = 0;
                        Customer customerTemp = customerRepository.GetSpecifiedCustomerType2ByMoble(mobile);
                        if (customerTemp == null)  // 不存在该物业， 则创建一个 customer type 2
                        {
                            effectedCustomerId = customerRepository.CreateCustomer(customer);
                            if (effectedCustomerId == 0)
                            {
                                co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, effectedCustomerId, "Failed,can not save to the db");
                                return Request.CreateResponse(HttpStatusCode.OK, co_responseString);
                            }
                        }
                        else
                        {
                            // 该物业人员已经存在
                        }
                        
                    }

                    co_responseString = CommonUtility.FormatCampOwnerResponseString(0, 0, "Succeed,match wuye and community success!");
                    return Request.CreateResponse(HttpStatusCode.OK, co_responseString);

                }
            }
            catch (Exception ex)
            {
                co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
            }
        }


        // POST api/values H5 创建社区 调用 此接口
        [Route("CreateCampOwner")]
        [HttpPost]
        //[ActionName("CreateCampOwner")]
        public HttpResponseMessage CreateCampOwner([FromBody]CampOwnerAndCommunity item)
        {                                              
            try
            {
                if (item == null || item.campOwner == null || item.community == null)
                {                    
                    co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
                }
                else
                {
                    long effectedCampOwnerId = 0;

                    // 小区owner 是否已经存在
                    CampOwner co = campOwnerRepository.GetSpecifiedCampOwnerByMobile(item.campOwner.mobile);
                    if (co == null)  // 不存在 去创建
                    {
                        item.campOwner.campCodePrefix = item.campOwner.mobile;
                        item.campOwner.username = "zhijian";
                        item.campOwner.password = "zhijian";
                        effectedCampOwnerId = campOwnerRepository.CreateCampOwner(item.campOwner);
                        if (effectedCampOwnerId == 0)
                        {
                            co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, effectedCampOwnerId, "Failed,can not save to the db");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
                        }
                        else
                        {
                            // 进入下面创建社区的逻辑
                        }

                    }
                    else  // 此小区owner存在， 直接返回
                    {
                        // try to create community with campOwner prefix code

                        // 进入下面创建社区的逻辑
                    }

                    // 创建社区

                    long effectedCommunityId = 0;
                    item.community.campCode = item.campOwner.campCodePrefix + CommonUtility.GenerateValidationCode();
                    item.community.campOwnerMobile = item.campOwner.mobile;
                    effectedCommunityId = communityRepositoy.CreateCommunity(item.community);
                    if (effectedCommunityId == 0)
                    {
                        communityResponseString = CommonUtility.FormatCommunityResponseString(-1, effectedCommunityId, "Failed,can not save to the db");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, communityResponseString);
                    }
                    else
                    {
                        communityResponseString = CommonUtility.FormatCommunityResponseString(0, effectedCommunityId, item.community.campCode);
                        return Request.CreateResponse(HttpStatusCode.Created, communityResponseString);
                    }

                }
            }
            catch (Exception ex)
            {
                co_responseString = CommonUtility.FormatCampOwnerResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, co_responseString);
            }

        }


    }
}

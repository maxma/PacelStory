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
    [RoutePrefix("api/v1/CustomerType2")]
    public class CustomerType2V1Controller : ApiController
    {
        ResponseString rs = new ResponseString();
        // GET: /Customer/
        CustomerRepository cr = new CustomerRepository();
        CustomerResponseString crs = new CustomerResponseString();

        PacelRepository pr = new PacelRepository();
        PacelResponseString prs = new PacelResponseString();

        CommunityRepository communityRepo = new CommunityRepository();

        // POST api/values 物业人员 输入 电话号码，快递单号，选择具体哪个小区 后 调用这个接口
        [Route("CreatePacelCustomer")]
        [HttpPost]
        //[ActionName("CreatePacelCustomer")]
        public HttpResponseMessage CreatePacelAndCustomer([FromBody]PacelAndCustomer pacelAndCustomer)
        {
            try
            {
                long effectedCustomerId = 0;
                long returnPacelId = 0;

                if (pacelAndCustomer == null || pacelAndCustomer.pacel == null || pacelAndCustomer.customer.mobile == null || pacelAndCustomer.pacel.logisticsId == null || pacelAndCustomer.customer.mobile.Trim() == "" || pacelAndCustomer.pacel.logisticsId.Trim() == "")
                {
                    PacelAndCustomer item = new PacelAndCustomer();
                    rs = CommonUtility.FormatResponseString(-1, "Failed,cannot read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, rs);
                }
                else
                {
                    // 利用物业人员数据，创建本社区用户
                    CustomerRepository cr = new CustomerRepository();

                    // PacelRepository pr = new PacelRepository();
                    Customer tempCustomer = cr.GetSpecifiedCustomerByMoble(pacelAndCustomer.customer.mobile);

                    // 获取物业人员信息
                    Customer wuye = cr.GetSpecifiedCustomerType2ByMoble(pacelAndCustomer.wuyeMobile);
                    if(wuye == null)
                    {
                        rs = CommonUtility.FormatResponseString(-1, "Failed,wuye does not exist!");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, rs);
                    }

                    if (tempCustomer == null)
                    {
                        // 利用物业人员信息，创建用户信息，只有电话使用用户的，其他信息使用物业人员的
                        tempCustomer = wuye;
                        tempCustomer.type = "0";
                        tempCustomer.groupName = pacelAndCustomer.groupName;
                        tempCustomer.mobile = pacelAndCustomer.customer.mobile;
                        effectedCustomerId = cr.CreateCustomer(tempCustomer);
                        if (effectedCustomerId == 0)
                        {
                            rs = CommonUtility.FormatResponseString(-1, "Failed,pacel or customer cannot be created!");
                            return Request.CreateResponse(HttpStatusCode.NotModified, rs);
                        }
                    }
                    else
                    {
                        // 用户已经存在， 直接存储 包裹, 存储包裹之前，将存在用户的customerId 赋给pacel对象里的 customerId
                        // 直接出去创建
                        effectedCustomerId = tempCustomer.customerId;
                    }

                    PacelRepository pr = new PacelRepository();
                    Pacel tempPacel = pr.GetPacelByLogisticsId(pacelAndCustomer.pacel.logisticsId, effectedCustomerId);

                    if (tempPacel == null)
                    {
                        // 直接存储 包裹, 存储包裹之前，将存在用户的customerId 赋给pacel对象里的 customerId
                        pacelAndCustomer.pacel.customerId = effectedCustomerId;
                        pacelAndCustomer.pacel.type = "0";
                        pacelAndCustomer.pacel.groupName = pacelAndCustomer.groupName;
                        pacelAndCustomer.pacel.province = wuye.province;
                        pacelAndCustomer.pacel.city = wuye.city;
                        pacelAndCustomer.pacel.district = wuye.district;
                        pacelAndCustomer.pacel.campname = wuye.campname;
                        pacelAndCustomer.pacel.arrivedDate = DateTime.Now;
                        pacelAndCustomer.pacel.communityId = wuye.communityId;
                        pacelAndCustomer.pacel.wuyeId = wuye.customerId;

                        // 生成二维码文件名 文件名以用户id + 日期命名
                        string codename = effectedCustomerId.ToString() + "qrcode";
                        string filename = CommonUtility.GenerateUniqueFileName(codename);

                        // 存储二维码文件名
                        pacelAndCustomer.pacel.twoDCode = CommonUtility.serverImageUrl + filename;

                        // 创建包裹
                        returnPacelId = pr.CreatePacel(pacelAndCustomer.pacel);

                        // 生成二维码内容
                        string codeContent = CommonUtility.ipAddress + ":81/api/Pacel/SignPacel/" + returnPacelId.ToString() + "/" + effectedCustomerId.ToString();

                        // 生成二维码文件在固定文件夹下
                        QrCodeUtility arCode = new QrCodeUtility();
                        arCode.GenerateQrCode(codeContent, filename);
                    }
                    else
                    {
                        // 该用户包裹已经存在，只赋值 pacelId
                        returnPacelId = tempPacel.pacelId;
                    }

                    // 发 物流包裹 短信  
                    #region send pacel text

                    Community community = communityRepo.GetSpecifiedCommunityById((long)wuye.communityId);
                    if(community == null)
                    {
                        rs = CommonUtility.FormatResponseString(-1, "Failed,community does not exist!");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, rs);
                    }
                    else
                    {
                        wuye.campname = wuye.campname == null ? "" : wuye.campname;
                        community.communityService = community.communityService == null ? "" : community.communityService;
                        string content = wuye.campname + community.communityService + "提醒您, 您有一个包裹已经达到，请安排好时间及时领取。" + CommonUtility.productName + " " + community.communityService + "已经升级，下载手机应用查看包裹信息 " + CommonUtility.downloadUrl;
                        CommonUtility.SendText(tempCustomer.mobile, "", "", content);
                    }

                    
                    
                    //TextRepository tr = new TextRepository();
                    //// struct textFormat
                    //tbl_smsmt_send textFormat = new tbl_smsmt_send();
                    //textFormat.account = "2521494";
                    //textFormat.mobile = tempCustomer.mobile;

                    //string productName = "【指尖社区】";
                 
                    
                    //textFormat.content = pacelAndCustomer.customer.campname + "物业提醒您, 您有一个包裹已经达到物业，请安排好时间及时领取。" + productName + " 物业已经升级，下载手机应用查看包裹信息 " + CommonUtility.downloadUrl;
                    //textFormat.smsid = "0567898f30e658dbff5a";
                    //textFormat.priority = 1;
                    //textFormat.SubmitTime = DateTime.Now;

                    //int sendSuccess = tr.CreateText(textFormat);
                    #endregion

                    if (effectedCustomerId != 0 && returnPacelId != 0)
                    {
                        rs = CommonUtility.FormatResponseString(0, "Succeed,pacel and customer created!");
                        return Request.CreateResponse(HttpStatusCode.Created, rs);
                    }
                    else
                    {
                        rs = CommonUtility.FormatResponseString(-1, "Failed,pacel or customer cannot be created!");
                        return Request.CreateResponse(HttpStatusCode.NotModified, rs);
                    }
                }

            }
            catch (Exception ex)
            {
                rs = CommonUtility.FormatResponseString(-1, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, rs);
            }


        }

        // POST api/values  物业版 开机 输入 电话号码 调用 此接口
        [Route("CheckCustomerType2/{mobile}")]
        [HttpGet]
        //[ActionName("CheckCustomerType2")]
        [Authorize]
        public HttpResponseMessage CheckCustomerType2(string mobile)
        {
            try
            {
                if (mobile == null || mobile.Trim() == "")
                {
                    crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    if (CommonUtility.IsDigit09(mobile))
                    {

                        Customer temp = cr.GetSpecifiedCustomerType2ByMoble(mobile);
                        if (temp == null)
                        {
                            crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,the customer type2 does not exist");
                            return Request.CreateResponse(HttpStatusCode.OK, crs);
                        }
                        else
                        {
                            crs = CommonUtility.FormatCustomerResponseString(0, temp.customerId, "Succeed,the customer type2 existed");
                            return Request.CreateResponse(HttpStatusCode.OK, crs);
                        }
                    }
                    else
                    {
                        crs = CommonUtility.FormatCustomerResponseString(-1, 0, "Failed,the mobile is not type number");
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


        // POST api/values  获取物业人员所在小区 有多少片区 调用 此接口
        [Route("GetGroupNames/{wuyeMobile}")]
        [HttpGet]
        //[ActionName("GetGroupNames")]
        //[Authorize]
        public List<string> GetGroupNames(string wuyeMobile)
        {
            try
            {
                List<string> groupNames = new List<string>();

                if (wuyeMobile == null || wuyeMobile.Trim() == "")
                {                    
                    return null;
                }
                else
                {
                    if (CommonUtility.IsDigit09(wuyeMobile) == true)
                    {
                        Customer tempCustomer = cr.GetSpecifiedCustomerType2ByMoble(wuyeMobile);
                        if (tempCustomer == null)
                        {
                            return null;
                        }
                        else
                        {
                            Community community = communityRepo.GetSpecifiedCommunityById((long)tempCustomer.communityId);
                            if (community.groupName1 != "" && community.groupName1 != null) groupNames.Add(community.groupName1);
                            if (community.groupName2 != "" && community.groupName2 != null) groupNames.Add(community.groupName2);
                            if (community.groupName3 != "" && community.groupName3 != null) groupNames.Add(community.groupName3);
                            if (community.groupName4 != "" && community.groupName4 != null) groupNames.Add(community.groupName4);
                            if (community.groupName5 != "" && community.groupName5 != null) groupNames.Add(community.groupName5);
                            if (community.groupName6 != "" && community.groupName6 != null) groupNames.Add(community.groupName6);
                            if (community.groupName7 != "" && community.groupName7 != null) groupNames.Add(community.groupName7);
                            if (community.groupName8 != "" && community.groupName8 != null) groupNames.Add(community.groupName8);
                            if (community.groupName9 != "" && community.groupName9 != null) groupNames.Add(community.groupName9);

                            // 如果小区一个片区都没有，就以小区 作为 片区 返回，因为小区同时也是片区
                            if(groupNames.Count == 0)
                            {
                                groupNames.Add(community.campname);
                            }

                            return groupNames;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        // GET api/values      
        [Route("SignPacel/{pacelId}/{customerId}")]
        [HttpGet]
        //[ActionName("SignPacel")]
        [Authorize]
        public HttpResponseMessage SignPacel(long pacelId, long customerId)
        {
            try
            {
                if (pacelId <= 0 || customerId <= 0)  // no pacel to update
                {
                    prs = CommonUtility.FormatPacelResponseString(-1, 0, "can not read object from body");

                    return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                }
                else
                {
                    // try to retrieve pacel in db.
                    Pacel dbPacel = pr.GetSpecifiedPacel(pacelId);

                    // update a non-existed pacel
                    if (dbPacel == null || dbPacel.customerId != customerId)
                    {
                        prs = CommonUtility.FormatPacelResponseString(-1, 0, "Pacel not match");

                        return Request.CreateResponse(HttpStatusCode.NotModified, prs);
                    }
                    else
                    {
                        if (dbPacel.signDate != null)
                        {
                            prs = CommonUtility.FormatPacelResponseString(0, dbPacel.pacelId, "already signed before");
                            return Request.CreateResponse(HttpStatusCode.OK, prs);
                        }
                        else
                        {

                            int effectedRows = 0;
                            dbPacel.signDate = DateTime.Now;
                            dbPacel.signname = customerId.ToString();
                            dbPacel.type = "1";
                            effectedRows = pr.UpdatePacel(dbPacel);
                            if (effectedRows == 0)
                            {
                                prs = CommonUtility.FormatPacelResponseString(-1, 0, "can not save to the db");

                                return Request.CreateResponse(HttpStatusCode.NotModified, prs);
                            }
                            else
                            {
                                prs = CommonUtility.FormatPacelResponseString(0, dbPacel.pacelId, "update items in db success");
                                return Request.CreateResponse(HttpStatusCode.OK, prs);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                prs = CommonUtility.FormatPacelResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
            }

        }


    }
}

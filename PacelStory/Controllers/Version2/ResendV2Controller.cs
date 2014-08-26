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
    [RoutePrefix("api/v2/Resend")]
    public class ResendV2Controller : ApiController
    {

        ResendRepository resendRepositoy = new ResendRepository();
        PacelRepository pacelRepository = new PacelRepository();
        CustomerRepository customerRepository = new CustomerRepository();
        CommunityRepository communityRepo = new CommunityRepository();

        PacelResponseString prs = new PacelResponseString();

        [Route("ReText/{pacelId}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage ResendText(long pacelId)
        {
            long resendReturnedId = 0;
            try
            {
                if (pacelId > 0)
                {
                    // 在Pacel表里找到这条记录，后面会根据 pacel表里的 customerId 获取targetMobile, 获取campName
                    Pacel pacel = pacelRepository.GetSpecifiedPacel(pacelId);
                    if (pacel == null)
                    {
                        prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,pacel does not exist");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                    }
                    else
                    {
                        Customer customer = customerRepository.GetSpecifiedCustomerById((long)pacel.customerId);
                        Customer wuye = customerRepository.GetSpecifiedCustomerType2ById((long)pacel.wuyeId);
                        if (customer == null || wuye == null) // 如果用户不存在，返回错误
                        {
                            prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,wuye or customer does not exist");
                            return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                        }
                        else
                        {
                            // 获取resend表里信息，看是否是第一次催促
                            Resend resend = resendRepositoy.GetSpecifiedResendByPacelId(pacelId);

                            Community community = communityRepo.GetSpecifiedCommunityById((long)pacel.wuyeId);

                            if (resend == null)   // 如果这个包裹是第一次催促拿取，在数据库中创建一条记录
                            {
                                Resend resendCreated = new Resend();
                                resendCreated.pacelId = pacelId;
                                resendCreated.resendTime = DateTime.Now;
                                resendReturnedId = resendRepositoy.CreateResend(resendCreated);
                                if (resendReturnedId > 0)
                                {
                                    // 发送一条短信

                                    wuye.campname = wuye.campname == null ? "" : wuye.campname;
                                    community.communityService = community.communityService == null ? "" : community.communityService;
                                    string messageText = wuye.campname + community.communityService + "希望您快点来领包裹啦, 您有一个包裹存在" + community.communityService + "超过1天，请安排好时间及时领取。" + CommonUtility.productName + " " + community.communityService + "下载手机应用查看快递信息 " + CommonUtility.downloadUrl + " " + CommonUtility.productName;
                                    CommonUtility.SendText(customer.mobile, "", "", messageText);

                                    prs = CommonUtility.FormatPacelResponseString(0, pacelId, "Succeed,created items in db success");
                                    return Request.CreateResponse(HttpStatusCode.Created, prs);
                                }
                                else
                                {
                                    prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,can not save to the db");
                                    return Request.CreateResponse(HttpStatusCode.OK, prs);
                                }
                            }
                            // 如果这个包裹是重复催促拿取，在数据库中更新一下催促拿取的时间
                            else
                            {
                                TimeSpan span = DateTime.Now.Subtract((DateTime)resend.resendTime);
                                if (span.Days >= 1)  // 一个自然天内只能催促一次
                                {
                                    // 发送一条短信
                                    wuye.campname = wuye.campname == null ? "" : wuye.campname;
                                    community.communityService = community.communityService == null ? "" : community.communityService;
                                    string messageText = wuye.campname + community.communityService + "希望您快点来领包裹啦, 您有一个包裹存在" + community.communityService + "超过1天，请安排好时间及时领取。" + CommonUtility.productName + " " + community.communityService + "下载手机应用查看快递信息 " + CommonUtility.downloadUrl + " " + CommonUtility.productName;
                                    CommonUtility.SendText(customer.mobile, "", "", messageText);

                                    prs = CommonUtility.FormatPacelResponseString(0, pacelId, "Succeed,resend text");
                                    return Request.CreateResponse(HttpStatusCode.Created, prs);
                                }
                                else
                                {
                                    // 一个自然天内催促多次，返回failed
                                    prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,one day one text");
                                    return Request.CreateResponse(HttpStatusCode.OK, prs);
                                }
                            }
                        }

                    }

                }
                else
                {
                    prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,param not invalid");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                }
            }
            catch
            {
                prs = CommonUtility.FormatPacelResponseString(-1, pacelId, "Failed,exception happens");
                return Request.CreateResponse(HttpStatusCode.OK, prs);
            }

        }

    }
}

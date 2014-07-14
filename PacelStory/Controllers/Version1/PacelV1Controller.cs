using PacelStory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using PacelStory.Repositories;
using PacelStory.Utilities;

namespace PacelStory.Controllers
{
    [RoutePrefix("api/v1/Pacel")]
    public class PacelV1Controller : ApiController
    {
        //
        // GET: /Pacel/

        PacelRepository pr = new PacelRepository();
        PacelResponseString prs = new PacelResponseString();

        private bool IsParamValid(long customerId, int pageNumber)
        {
            if (customerId >= 1 && pageNumber >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // GET api/get 用户版调用 未签收包裹 调用此接口
        [Route("UnSigned/{customerId}/{pageNumber}")]
        [HttpGet]
        //[ActionName("UnSigned")]
        public List<Pacel> GetUnSignedPacelsByCustomerId(long customerId, int pageNumber)
        {
            if (!IsParamValid(customerId, pageNumber))
            {
                return null;
            }
            else
            {
                return pr.GetUnSignedPacelsByCustomerId(customerId, pageNumber).ToList();
            }
        }

        // *************物业版 调用 此小区 还有哪些 未签收包裹，调用此接口
        [Route("GetUnSignedPacelsByCustomerType2Id/{customerType2Id}/{pageNumber}")]
        [HttpGet]
        [Authorize]
        public List<Pacel> GetUnSignedPacelsByCustomerType2Id(long customerType2Id, int pageNumber)
        {
            if (!IsParamValid(customerType2Id, pageNumber))
            {
                return null;
            }
            else
            {
                return pr.GetUnSignedPacelsByCustomerType2Id(customerType2Id, pageNumber).ToList();
                
            }
        }

        /*
        // GET api/get 
        [Route("api/Pacel/GetUnsignedPacelsByCommunityId/{communityId}/{pageNumber}")]
        [HttpGet]
        [Authorize]
        //[ActionName("UnSigned")]
        public List<Pacel> GetUnsignedPacelsByCommunityId(long communityId, int pageNumber)
        {
            if (!IsParamValid(communityId, pageNumber))
            {
                return null;
            }
            else
            {
                return pr.GetUnSignedPacelsByCommunityId(communityId, pageNumber).ToList();
            }
        }
        */


        [Route("Signed/{customerId}/{pageNumber}")]
        [HttpGet]
        //[ActionName("Signed")]
        public List<Pacel> GetSignedPacelsByCustomerId(long customerId, int pageNumber)
        {
            if (!IsParamValid(customerId, pageNumber))
            {
                return null;
            }
            else
            {
                return pr.GetSignedPacelsByCustomerId(customerId, pageNumber).ToList();
            }
        }

        [Route("Pacels/{customerId}/{pageNumber}")]
        [HttpGet]
        //[ActionName("Pacels")]
        public List<Pacel> GetPacelsByCustomerId(long customerId, int pageNumber)
        {
            if (!IsParamValid(customerId, pageNumber))
            {
                return null;
            }
            else
            {
                return pr.GetPacelsByCustomerId(customerId, pageNumber).ToList();
            }
        }

        [Route("SpecifiedPacel/{pacelId}")]
        [HttpGet]
        //[ActionName("SpecifiedPacel")]
        public Pacel GetSpecifiedPacel(long pacelId)
        {
            if (pacelId < 1)
            {
                return null;
            }
            else
            {
                return pr.GetSpecifiedPacel(pacelId);
            }
        }

        
        // POST api/values
        [Route("CreatePacel")]
        [HttpPost]
        //[ActionName("CreatePacel")]
        public HttpResponseMessage CreatePacel([FromBody]Pacel pacel)
        {            
            try
            {
                if (pacel == null)
                {
                    prs = CommonUtility.FormatPacelResponseString(-1, 0, "Failed,can not read object from body");
                                        
                    return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                }
                else
                {
                    long returnPacelId = 0;
                    pacel.arrivedDate = DateTime.Now;
                    returnPacelId = pr.CreatePacel(pacel);
                    if (returnPacelId == 0)
                    {
                        prs = CommonUtility.FormatPacelResponseString(-1, returnPacelId, "Failed,can not save to the db");

                        return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                    }
                    else
                    {
                        prs = CommonUtility.FormatPacelResponseString(0, returnPacelId, "Succeed,created items in db success");
                        return Request.CreateResponse(HttpStatusCode.Created, prs);
                    }

                }
            }
            catch (Exception ex)
            {
                prs = CommonUtility.FormatPacelResponseString(-1, 0, "Failed," + ex.Message);

                return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
            }


        }

        // GET api/values  
        [Route("SignPacel/{pacelId}/{customerId}")]
        [HttpGet]
        //[ActionName("SignPacel")]
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
                        if (Int32.Parse(dbPacel.type) == 1)
                        {
                            prs = CommonUtility.FormatPacelResponseString(0, dbPacel.pacelId, "already signed before");
                            return Request.CreateResponse(HttpStatusCode.OK, prs);
                        }
                        else
                        {
                            if (Int32.Parse(dbPacel.type) == 0)
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
                            else
                            {
                                prs = CommonUtility.FormatPacelResponseString(-1, dbPacel.pacelId, "unknown pacel type");
                                return Request.CreateResponse(HttpStatusCode.NotAcceptable, prs);
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


        // PUT api/values
        [Route("UpdatePacel")]
        [HttpPatch]
        [HttpPut]
        //[ActionName("UpdatePacel")]
        public HttpResponseMessage UpdatePacel([FromBody]Pacel pacel)
        {
            try
            {
                if (pacel == null)  // no pacel to update
                {
                    prs = CommonUtility.FormatPacelResponseString(-1, 0, "can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
                }
                else
                {
                    // try to retrieve pacel in db.
                    Pacel dbPacel = pr.GetSpecifiedPacel(pacel.pacelId);

                    // update a non-existed pacel
                    if (dbPacel == null || pacel.pacelId != dbPacel.pacelId)
                    {
                        prs = CommonUtility.FormatPacelResponseString(-1, 0, "Pacel not found");
                        return Request.CreateResponse(HttpStatusCode.NotModified, prs);
                    }
                    else
                    {
                        // customer signed for this pacel, remove from table Pacel and add this record to table SignedPacel
                        //if (pacel.type == "1" && dbPacel.type == "0")
                        //{

                        //}

                        int effectedRows = 0;
                        effectedRows = pr.UpdatePacel(pacel);
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
            catch (Exception ex)
            {
                prs = CommonUtility.FormatPacelResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, prs);
            }

        }

        [Route("RemovePacel/{id}")]
        [HttpDelete]
        public HttpResponseMessage RemovePacel(long id)
        {
            try
            {
                // try to retrieve pacel in db.
                var pacel = pr.GetSpecifiedPacel(id);

                if (pacel == null)  // no pacel to delete
                {
                    prs = CommonUtility.FormatPacelResponseString(-1, 0, "can not find target you delete");
                    return Request.CreateResponse(HttpStatusCode.NotFound, prs);
                }
                else
                {

                    int effectedRows = 0;
                    effectedRows = pr.RemovePacel(pacel.pacelId);
                    if (effectedRows == 0)
                    {
                        prs = CommonUtility.FormatPacelResponseString(-1, 0, "can not delete from the db");
                        return Request.CreateResponse(HttpStatusCode.NotModified, prs);
                    }
                    else
                    {
                        prs = CommonUtility.FormatPacelResponseString(0, 0, "delete items in db success");
                        return Request.CreateResponse(HttpStatusCode.OK, "Code:0; delete items in db success.");
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PacelStory.Repositories;
using PacelStory.Models;
using System.Net;
using System.Web.Http;
using System.Net.Http;
using PacelStory.Utilities;
using System.Text.RegularExpressions;

namespace PacelStory.Controllers
{
    [RoutePrefix("api/v1/Community")]
    public class CommunityV1Controller : ApiController
    {
        //
        // GET: /Community/

        CommunityRepository cr = new CommunityRepository();
        CommunityResponseString crs = new CommunityResponseString();

        CustomerRepository customerRepo = new CustomerRepository();


        // GET api/get 
        [Route("AllCommunities/{pageNumber}")]
        [HttpGet]
        //[ActionName("AllCommunities")]
        public List<Community> GetAllCommunities(int pageNumber)
        {
            if (pageNumber >= 1)
            {
                return cr.GetAllCommunities(pageNumber).ToList();
            }
            else
            {
                return null;
            }
        }

        [Route("SpecifiedCommunityById/{customerId}")]
        [HttpGet]
        //[ActionName("SpecifiedCommunityById")]
        public Community GetSpecifiedCommunity(long customerId)
        {
            long communityId = 0;

            if(customerId >= 1)
            {
                Customer customer = customerRepo.GetSpecifiedCustomerById(customerId);
                if (customer.communityId != null && customer.communityId != 0)
                {
                    communityId = (long)customer.communityId;
                }
                else
                {
                    return null;
                }
            }

            if (communityId >= 1)
            {
                return cr.GetSpecifiedCommunityById(communityId);
            }
            else
            {
                return null;
            }
        }

        // H5关联物业 进入 页面，则调用此接口
        [Route("SpecifiedCommunityByMobile/{mobile}")]
        [HttpGet]
        //[ActionName("SpecifiedCommunityByMobile")]
        public List<Community> GetCommunityByCampOwnerMobile(string mobile)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(mobile))
            {
                return cr.GetCommunityByCampOwnerMobile(mobile);
            }
            else
            {
                return null;
            }
        }


        // POST api/values
        [Route("CreateCommunity")]
        [HttpPost]
        //[ActionName("CreateCommunity")]
        public HttpResponseMessage CreateCommunity([FromBody]Community community)
        {
            try
            {
                if (community == null)
                {
                    crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    long effectedCommunityId = 0;
                    effectedCommunityId = cr.CreateCommunity(community);
                    if (effectedCommunityId == 0)
                    {
                        crs = CommonUtility.FormatCommunityResponseString(-1, effectedCommunityId, "Failed,can not save to the db");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                    }
                    else
                    {
                        crs = CommonUtility.FormatCommunityResponseString(0, effectedCommunityId, "Succeed,created items in db success");
                        return Request.CreateResponse(HttpStatusCode.Created, crs);
                    }

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }


        }

        // PUT api/values
        [Route("UpdateCommunity")]
        [HttpPatch]
        [HttpPut]
        //[ActionName("UpdateCommunity")]
        public HttpResponseMessage UpdateCommunity([FromBody]Community community)
        {
            try
            {
                if (community == null)  // no pacel to update
                {
                    crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,can not read object from body");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
                }
                else
                {
                    // try to retrieve pacel in db.
                    Community dbCommunity = cr.GetSpecifiedCommunityById(community.communityId);

                    // update a non-existed pacel
                    if (dbCommunity == null || community.communityId != dbCommunity.communityId)
                    {
                        crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,target object not found");
                        return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                    }
                    else
                    {

                        long effectedCommunityId = 0;
                        effectedCommunityId = cr.UpdateCommunity(community);
                        if (effectedCommunityId == 0)
                        {
                            crs = CommonUtility.FormatCommunityResponseString(-1, effectedCommunityId, "Failed,can not save to the db");
                            return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                        }
                        else
                        {
                            crs = CommonUtility.FormatCommunityResponseString(0, effectedCommunityId, "Succeed,created items in db success");
                            return Request.CreateResponse(HttpStatusCode.OK, crs);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }

        }

        [Route("RemoveCommunity/{id}")]
        [HttpDelete]
        public HttpResponseMessage RemoveCommunity(long id)
        {
            try
            {
                if(id < 1)
                {
                    crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,id < 1");
                    return Request.CreateResponse(HttpStatusCode.NotFound, crs);
                }

                // try to retrieve pacel in db.
                var community = cr.GetSpecifiedCommunityById(id);

                if (community == null)  // no community to delete
                {
                    crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,can not find target you delete");
                    return Request.CreateResponse(HttpStatusCode.NotFound, crs);
                }
                else
                {

                    int effectedRows = 0;
                    effectedRows = cr.RemoveCommunity(community.communityId);
                    if (effectedRows == 0)
                    {
                        crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed,can not delete from the db");
                        return Request.CreateResponse(HttpStatusCode.NotModified, crs);
                    }
                    else
                    {
                        crs = CommonUtility.FormatCommunityResponseString(0, effectedRows, "Failed,delete items in db success");
                        return Request.CreateResponse(HttpStatusCode.OK, crs);
                    }

                }
            }
            catch (Exception ex)
            {
                crs = CommonUtility.FormatCommunityResponseString(-1, 0, "Failed," + ex.Message);
                return Request.CreateResponse(HttpStatusCode.BadRequest, crs);
            }
        }
           

    }
}

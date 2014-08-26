using PacelStory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PacelStory.Controllers
{
    [RoutePrefix("api/v2/Upgrade")]
    public class UpgradeV2Controller : ApiController
    {
        UpgradeResponseString urs = new UpgradeResponseString();

        [Route("IsUpgradeIOS/{clientCurrentVersionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeIOS(string clientCurrentVersionNumber)
        {
            try
            {     
                int currentVersionNumber = 100;
                bool needUpgrade = false;

                if (currentVersionNumber > Int16.Parse(clientCurrentVersionNumber))
                {
                    needUpgrade = true;
                }

                if (needUpgrade == true)
                {
                    urs = CommonUtility.FormatUpgradeResponseString(0, -1, "upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
                else
                {
                    urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "not upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
            }
            catch
            {
                urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "exception occurs");
                return Request.CreateResponse(HttpStatusCode.BadRequest, urs);
            }
            
        }

        [Route("IsUpgradeAndroid/{clientCurrentVersionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeAndroid(string clientCurrentVersionNumber)
        {
            try
            {
                int currentVersionNumber = 100;
                bool needUpgrade = false;

                if (currentVersionNumber > Int16.Parse(clientCurrentVersionNumber))
                {
                    needUpgrade = true;
                }

                if (needUpgrade == true)
                {
                    urs = CommonUtility.FormatUpgradeResponseString(0, -1, "upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
                else
                {
                    urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "not upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
            }
            catch
            {
                urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "exception occurs");
                return Request.CreateResponse(HttpStatusCode.BadRequest, urs);
            }

        }

        [Route("IsUpgradeIOSForWuye/{clientCurrentVersionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeIOSForWuye(string clientCurrentVersionNumber)
        {
            try
            {
                int currentVersionNumber = 100;
                bool needUpgrade = false;

                if (currentVersionNumber > Int16.Parse(clientCurrentVersionNumber))
                {
                    needUpgrade = true;
                }

                if (needUpgrade == true)
                {
                    urs = CommonUtility.FormatUpgradeResponseString(0, -1, "upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
                else
                {
                    urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "not upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
            }
            catch
            {
                urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "exception occurs");
                return Request.CreateResponse(HttpStatusCode.BadRequest, urs);
            }

        }

        [Route("IsUpgradeAndroidForWuye/{clientCurrentVersionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeAndroidForWuye(string clientCurrentVersionNumber)
        {
            try
            {
                int currentVersionNumber = 100;
                bool needUpgrade = false;

                if (currentVersionNumber > Int16.Parse(clientCurrentVersionNumber))
                {
                    needUpgrade = true;
                }

                if (needUpgrade == true)
                {
                    urs = CommonUtility.FormatUpgradeResponseString(0, -1, "upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
                else
                {
                    urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "not upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, urs);
                }
            }
            catch
            {
                urs = CommonUtility.FormatUpgradeResponseString(-1, -1, "exception occurs");
                return Request.CreateResponse(HttpStatusCode.BadRequest, urs);
            }

        }

        // not use
        private short[] String2IntArray(string versionNumber)
        {            
            string[] stringArray = versionNumber.Split('.');
            short[] intArray = Array.ConvertAll(stringArray, id => Convert.ToInt16(id));

            return intArray;
        }
    }
}

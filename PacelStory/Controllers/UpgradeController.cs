using PacelStory.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PacelStory.Controllers
{
    public class UpgradeController : ApiController
    {
        UpgradeResponseString urs = new UpgradeResponseString();

        [Route("api/Upgrade/IsUpgradeIOS/{versionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeIOS(string versionNumber)
        {
            try
            {     
                int oldVersionNumber = 100;
                bool needUpgrade = false;

                if(oldVersionNumber < Int16.Parse(versionNumber))
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

        [Route("api/Upgrade/IsUpgradeAndroid/{versionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeAndroid(string versionNumber)
        {
            try
            {
                int oldVersionNumber = 100;
                bool needUpgrade = false;

                if (oldVersionNumber < Int16.Parse(versionNumber))
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

        [Route("api/Upgrade/IsUpgradeIOSForWuye/{versionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeIOSForWuye(string versionNumber)
        {
            try
            {
                int oldVersionNumber = 100;
                bool needUpgrade = false;

                if (oldVersionNumber < Int16.Parse(versionNumber))
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

        [Route("api/Upgrade/IsUpgradeAndroidForWuye/{versionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgradeAndroidForWuye(string versionNumber)
        {
            try
            {
                int oldVersionNumber = 100;
                bool needUpgrade = false;

                if (oldVersionNumber < Int16.Parse(versionNumber))
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

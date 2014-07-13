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
        ResponseString rs = new ResponseString();

        [Route("api/Upgrade/IsUpgrade/{versionNumber}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage IsUpgrade(string versionNumber)
        {
            try
            {
                string oldVersionNumber = "1.0.0";
                bool needUpgrade = false;

                short[] oldNumber = String2IntArray(oldVersionNumber);
                short[] newNumber = String2IntArray(versionNumber);

                for (int i = 0; i < oldNumber.Length && i < newNumber.Length; i++)
                {
                    if (oldNumber[i] < newNumber[i])
                    {
                        needUpgrade = true;
                    }
                }

                if (needUpgrade == true)
                {
                    rs = CommonUtility.FormatResponseString(0, "upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, rs);
                }
                else
                {
                    rs = CommonUtility.FormatResponseString(-1, "not upgraded");
                    return Request.CreateResponse(HttpStatusCode.OK, rs);
                }
            }
            catch
            {
                rs = CommonUtility.FormatResponseString(-1, "exception occurs");
                return Request.CreateResponse(HttpStatusCode.BadRequest, rs);
            }
            
        }

        private short[] String2IntArray(string versionNumber)
        {            
            string[] stringArray = versionNumber.Split('.');
            short[] intArray = Array.ConvertAll(stringArray, id => Convert.ToInt16(id));

            return intArray;
        }
    }
}

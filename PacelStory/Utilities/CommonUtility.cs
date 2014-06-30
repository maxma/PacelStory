using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;


namespace PacelStory.Utilities
{
    public static class CommonUtility
    {
        public static string ipAddress = "http://122.10.117.234";
        public static string serverImageUrl = ipAddress + ":81/Images/QrCode/";
        public static string downloadUrl = ipAddress + ":81/PackageDownload/Package";

        // 生成唯一文件名
        public static string GenerateUniqueFileName(string filename)
        {
            string[] paths = filename.Split('.');
            string time = DateTime.Now.ToString();
            time = time.Replace(" ", "-");
            time = time.Replace(":", "-");
            time = time.Replace("/", "-");

            string returnString = paths[0] + time + ".jpg";

            return returnString;
        }

        // generate 4-6 digit random number
        public static string GenerateValidationCode()
        {
            Random random = new Random();
            return random.Next(1000, 10000).ToString();
        }

        // 判断一个字符串是否是0-9数字组成
        public static bool IsDigit09(string mobile)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(mobile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // 格式化 CustomerController response string 
        public static CustomerResponseString FormatCustomerResponseString(int code, long customerId, string message)
        {
            CustomerResponseString crs = new CustomerResponseString();
            crs.code = code;
            crs.customerId = customerId;
            crs.message = message;

            return crs;
        }

        // 格式化 PacelController response string 
        public static PacelResponseString FormatPacelResponseString(int code, long pacelId, string message)
        {
            PacelResponseString prs = new PacelResponseString();
            prs.code = code;
            prs.pacelId = pacelId;
            prs.message = message;

            return prs;
        }

        public static CommunityResponseString FormatCommunityResponseString(int code, long communityId, string message)
        {
            CommunityResponseString crs = new CommunityResponseString();
            crs.code = code;
            crs.communityId = communityId;
            crs.message = message;

            return crs;
        }

        public static CampOwnerResponseString FormatCampOwnerResponseString(int code, long campOwnerId, string message)
        {
            CampOwnerResponseString cors = new CampOwnerResponseString();
            cors.code = code;
            cors.campOwnerId = campOwnerId;
            cors.message = message;

            return cors;
        }

        public static ResponseString FormatResponseString(int code, string message)
        {
            ResponseString rd = new ResponseString();
            rd.code = code;
 
            rd.message = message;

            return rd;
        }
      

    }

    public class ResponseString
    {
        public int code;
        public string message;
    }

    public class CustomerResponseString
    {
        public int code;
        public long customerId;
        public string message;
    }

    public class PacelResponseString
    {
        public int code;
        public long pacelId;
        public string message;
    }

    public class CommunityResponseString
    {
        public int code;
        public long communityId;
        public string message;
    }

    public class CampOwnerResponseString
    {
        public int code;
        public long campOwnerId;
        public string message;
    }


}
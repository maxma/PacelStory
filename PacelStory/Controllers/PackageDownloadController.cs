using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace PacelStory.Controllers
{
    public class PackageDownloadController : Controller
    {
        public ActionResult Package()
        {
            //one option
            // return RedirectToActionPermanent("ContactUs");
            //another option

            if (Request.Browser.IsMobileDevice)
            {
                if (Request.UserAgent.Contains("iPhone") || Request.UserAgent.Contains("iPod") || Request.UserAgent.Contains("iPad"))
                {

                    return Redirect("https://itunes.apple.com/us/app/zhi-jian-she-qu-yong-hu-ban/id888720769?l=zh&ls=1&mt=8");
                }
                else
                {
                    if(Request.UserAgent.Contains("Windows Phone"))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        return Redirect("http://122.10.117.234/APK/BaoGuo.apk");
                    }
                }
            }
            else
            {
                return Redirect("http://122.10.117.234/APK/BaoGuo.apk");
            }
                        
        }


        public ActionResult PackageForWuye()
        {
            //one option
            // return RedirectToActionPermanent("ContactUs");
            //another option

            if (Request.Browser.IsMobileDevice)
            {
                if (Request.UserAgent.Contains("iPhone") || Request.UserAgent.Contains("iPod") || Request.UserAgent.Contains("iPad"))
                {

                    return Redirect("https://itunes.apple.com/us/app/zhi-jian-she-qu-shang-hu-ban/id888729168?l=zh&ls=1&mt=8");
                }
                else
                {
                    if (Request.UserAgent.Contains("Windows Phone"))
                    {
                        return View("NotFound");
                    }
                    else
                    {
                        return Redirect("http://122.10.117.234/APK/Wuye.apk");
                    }
                }
            }
            else
            {
                return Redirect("http://122.10.117.234/APK/Wuye.apk");                
            }


        }

    }
}

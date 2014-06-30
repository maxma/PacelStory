using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace PacelStory.Utilities
{
    public class QrCodeUtility
    {

        public int GenerateQrCode(string codeTarget, string codeName)
        {

            var Mappingpath = System.Web.HttpContext.Current.Server.MapPath("~/Images/QrCode/" + codeName);

            IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE, Options = new EncodingOptions { Height = 300, Width=300 } };
            var result = writer.Write(codeTarget);
            var barcodeBitmap = new Bitmap(result);
            barcodeBitmap.Save(Mappingpath, System.Drawing.Imaging.ImageFormat.Jpeg);

            return 0;
        }
    }
}
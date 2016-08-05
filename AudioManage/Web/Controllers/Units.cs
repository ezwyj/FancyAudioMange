using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace Web.Controllers
{
    public static class Units
    {
        private static string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private static string appSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        //生成用户分享用二维码

        public static string UrlConvertToR(string imageurl1)
        {

            string tmpRootDir = HttpContext.Current.Server.MapPath(System.Web.HttpContext.Current.Request.ApplicationPath.ToString()); //获取程序根目录

            string imageurl2 = imageurl1.Replace(tmpRootDir, "");  //转换成相对路径

            imageurl2 = imageurl2.Replace(@"\", @"/");

            return imageurl2;

        }

        public static string GetPictureQrCode(string picUrl, string weixinId)
        {
            return GetPicture(picUrl, weixinId, "qrcode");
        }
        private static string GetPicture(string picUrl, string weixinId, string picType)
        {
            try
            {
                WebRequest request = WebRequest.Create(picUrl);
                WebResponse response = request.GetResponse();
                Stream reader = response.GetResponseStream();

                string fileName = weixinId + ".jpg";
                string savePath = HttpContext.Current.Request.MapPath("~/upFile/") + picType + "_" + fileName;
                FileStream writer = new FileStream(savePath, FileMode.OpenOrCreate, FileAccess.Write);
                byte[] buff = new byte[512];
                int c = 0; //实际读取的字节数
                while ((c = reader.Read(buff, 0, buff.Length)) > 0)
                {
                    writer.Write(buff, 0, c);
                }
                writer.Close();
                writer.Dispose();

                reader.Close();
                reader.Dispose();
                response.Close();
                return savePath;
            }
            catch
            {
                return "error";
            }


        }


       
    }
}
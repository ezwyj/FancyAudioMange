using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace AudioCore.Service
{
    public static class Units
    {
        private static string appId = ConfigurationManager.AppSettings["WeixinAppId"];
        private static string appSecret = ConfigurationManager.AppSettings["WeixinAppSecret"];
        //生成用户分享用二维码

        public static string GetPictureHead(string picUrl, string weixinId)
        {
            return GetPicture(picUrl, weixinId, "head");
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

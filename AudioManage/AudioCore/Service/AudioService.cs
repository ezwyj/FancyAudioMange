using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioCore.Entity;
using Log.Service;
using Log.Entity;
using System.Net;
using System.IO;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Web.Configuration;

namespace AudioCore.Service
{
    public class AudioService
    {

        private static string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private static string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        public static Entity.AudioEntity SaveAudion(string valueSetJson, out bool state, out string msg)
        {
            state = false;
            msg = string.Empty;
            AudioCore.Entity.AudioEntity entity = new Entity.AudioEntity();

            try
            {
                entity = Serializer.ToObject<AudioCore.Entity.AudioEntity>(valueSetJson);
                entity.CreateTime = DateTime.Now;
               
                string log = string.Empty;
                state = entity.Save(out msg);
                

                if (!state)
                {
                    LogService.WriteLog(LogTypeEnum.错误日志, "保存音频文件", msg);
                }
                return entity;
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return entity;
        }

        public static string BuildQrCode(int audioId)
        {
            CreateQrCodeResult qrResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.CreateByStr(appId, audioId.ToString());
            string QrCodeURL = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            string localUrl = GetPictureQrCode(QrCodeURL, audioId.ToString());
            return localUrl;
        }

        private static string GetPictureQrCode(string QrCodeURL, string AudioId)
        {
            try
            {
                WebRequest request = WebRequest.Create(QrCodeURL);
                WebResponse response = request.GetResponse();
                Stream reader = response.GetResponseStream();

                string fileName = AudioId + ".jpg";
                //string savePath = HttpContext.Current.Request.MapPath("~/upFile/") + "qrCode_" + fileName;
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

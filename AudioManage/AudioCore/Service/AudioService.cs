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

using System.Web.Configuration;

namespace AudioCore.Service
{
    public class AudioService
    {

        private static string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private static string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        public static Entity.AudioEntity SaveAudio(AudioEntity entity, out bool state, out string msg)
        {
            state = false;
            msg = string.Empty;
           

            try
            {
                
                entity.CreateTime = DateTime.Now;
                if( entity.Id==0)
                {
                    entity.QrCodeFile = DateTime.Now.ToString("yyyyMMddHHmmssffff")+".jpg";
                }
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

        

        
    }

}

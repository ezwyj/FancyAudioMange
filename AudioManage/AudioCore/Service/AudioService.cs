using Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AudioCore.Entity;
using Log.Service;
using Log.Entity;

namespace AudioCore.Service
{
    public class AudioService
    {
        public static Entity.AudioEntity SaveAudion(string valueSetJson, out bool state, out string msg)
        {
            state = false;
            msg = string.Empty;
            AudioCore.Entity.AudioEntity entity = new Entity.AudioEntity();

            try
            {
                entity = Serializer.ToObject<AudioCore.Entity.AudioEntity>(valueSetJson);
                string log = string.Empty;
                state = entity.Save(out msg);

                if (!state)
                {
                    LogService.WriteLog(LogTypeEnum.错误日志, "保存音频文件", msg);
                }
                
            }
            catch (Exception e)
            {
                msg = e.Message;
            }

            return entity;
        }
    }
}

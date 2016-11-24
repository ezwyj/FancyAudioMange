using AudioCore.Entity;
using AudioCore.Service;
using Common.Util;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using Senparc.Weixin.MP.AdvancedAPIs;
using ValueSet.Service;


namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private static string appId = WebConfigurationManager.AppSettings["WeixinAppId"];
        private static string appSecret = WebConfigurationManager.AppSettings["WeixinAppSecret"];
        //
        // GET: /Home/

        public ActionResult Index()
        {
            string badge = HttpContext.User.Identity.Name;

            if (!Service.RightService.IsHaveRight(badge, "首页"))
            {
                Response.Redirect("~/Account/Login", true);
                return null;
            }

            return View();
        }
        private static string GetPictureQrCode(string QrCodeURL, string savePath)
        {
            try
            {
                WebRequest request = WebRequest.Create(QrCodeURL);
                WebResponse response = request.GetResponse();
                Stream reader = response.GetResponseStream();

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
            catch(Exception e)
            {
                string msg = e.Message;
                return "error";
            }
        }

        [HttpPost]
        public JsonResult SubmitQrcode(int audioId, string savefile)
        {
            bool state = true;
            string msg = string.Empty;

            try
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~\\Upload");
                string url = BuildQrCode(audioId, path+"\\"+savefile);
                return new JsonResult { Data = new { state = true, msg = url } };
               

            }
            catch (Exception e)
            {
                state = false;
                msg = e.Message;
            }

            return new JsonResult { Data = new { state = state, msg = msg } };
        }
        public static string BuildQrCode(int audioId, string savefile)
        {
            string mobileUrl = System.Configuration.ConfigurationManager.AppSettings["mobileUrl"];
        
             CreateQrCodeResult qrResult = Senparc.Weixin.MP.AdvancedAPIs.QrCodeApi.CreateByStr(appId, mobileUrl+audioId.ToString());
            string QrCodeURL = QrCodeApi.GetShowQrCodeUrl(qrResult.ticket);
            string localUrl = GetPictureQrCode(QrCodeURL,savefile);
            return localUrl;
        }
        
        [HttpPost]
        public JsonResult submitDelete(int audioId)
        {
            bool state=false;
            string msg = string.Empty;

            state = AudioService.Delete(audioId,out msg);

            return new JsonResult { Data = new { state = state, msg = msg} };
        }

        [HttpPost]
        public JsonResult Detial(string valueSetJson)
        {

            bool state = true;
            string msg = string.Empty;
            var postEntity = Serializer.ToObject<AudioCore.Entity.AudioEntity>(valueSetJson);
            int oldId = postEntity.Id;

            AudioEntity entity = AudioService.SaveAudio(postEntity, out state, out msg);
            if (state && oldId == 0)
            {
                string path = System.Web.HttpContext.Current.Server.MapPath("~\\Upload");
                BuildQrCode(entity.Id, path+"\\"+entity.QrCodeFile);
            }
            if (string.IsNullOrEmpty(msg)) msg = "保存成功";
            
            return new JsonResult { Data = new { state = state, msg = msg, data = entity } };
        }
        

        public ActionResult Detial(int id)
        {

            ViewBag.ClassOneList = ValueSetService.GetValueList(1, true);
            ViewBag.ClassTwoList = ValueSetService.GetValueList(2, true);
            ViewBag.ClassThreeList = ValueSetService.GetValueList(3, true);
            if (id == 0)
            {
                var ret = new AudioEntity();
                ret.CreateTime = DateTime.Now;
                ret.orderNum = 100;
                ret.Id = 0;
                return View(ret);
            }
            else
            {
                return View(AudioEntity.GetSingle(id));
            }
            
            
        }
        

        public JsonResult SearchList(int pageIndex, int pageSize,string title)
        {
            bool state = true;
            string msg = string.Empty;
            long total = 0;

            List<AudioEntity> serachList = null;
            Page<AudioEntity> Page = null;
            try
            {
                Sql sql = new Sql();
                sql.Append("select * from audio with(nolock) where 1=1 ");
                if (!string.IsNullOrEmpty(title)) sql.Append(" and title=@0", title.Replace("'", ""));
                sql.Append(" ORDER BY orderNum ");

                Page = AudioEntity.DefaultDB.Page<AudioEntity>(pageIndex, pageSize, sql);



            }
            catch (Exception e)
            {
                msg = e.Message;
            }
            total = Page.TotalItems;
            serachList = Page.Items;


            return new JsonResult { Data = new { state = state, msg = msg, data = serachList, total = total }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

    }
}

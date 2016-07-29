using AudioCore.Entity;
using AudioCore.Service;
using PetaPoco;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.QrCode;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

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
        [HttpPost]
        public JsonResult Detial(string valueSetJson)
        {

            bool state = true;
            string msg = string.Empty;

            AudioEntity entity = AudioService.SaveAudion(valueSetJson, out state, out msg);
            if (string.IsNullOrEmpty(msg)) msg = "增加成功";
            
            return new JsonResult { Data = new { state = state, msg = msg, data = entity } };
        }

        public ActionResult Detial(int id)
        {
            if (id == 0)
            {
                var ret = new AudioEntity();
                ret.CreateTime = DateTime.Now;
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
                sql.Append(" ORDER BY CreateTime DESC");

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

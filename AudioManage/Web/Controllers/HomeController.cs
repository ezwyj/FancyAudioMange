using AudioCore.Entity;
using AudioCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
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

    }
}

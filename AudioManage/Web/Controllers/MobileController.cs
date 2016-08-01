using AudioCore.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class MobileController : Controller
    {
        //
        // GET: /Mobile/

        public ActionResult Index(int id)
        {
            if (id != null)
            {
                return View(AudioEntity.GetSingle(id));
            }
            else
            {
                return View(AudioEntity.GetSingle(5));
            }
            
            
        }
    }
}

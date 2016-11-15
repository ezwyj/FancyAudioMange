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

        public ActionResult List(string classOne)
        {
            var entity = AudioEntity.GetListByProperty(a => a.ClassONE, classOne);
            return View(entity);
        }

        public ActionResult ListOne()
        {
            return List("01");
        }
        public ActionResult ListTwo()
        {
            return List("02");
        }
        public ActionResult ListThree()
        {
            return List("03");
        }

        public ActionResult Index(int id)
        {
            if (id != null)
            {
                var entity=  AudioEntity.GetSingle(id);
                List<string> play = new List<string>();
                foreach(var item in entity.AudioFile)
                {

                    string url =  item.FileAddress.Substring(item.FileAddress.IndexOf("Upload")).Replace('\\','/');
                    play.Add(url);
                }
                ViewBag.playList = string.Join(",", play);
                return View(entity);
            }
            else
            {
                
                return View(AudioEntity.GetSingle(13));
            }
            
            
        }
    }
}

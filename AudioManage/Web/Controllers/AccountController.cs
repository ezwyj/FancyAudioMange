using Account.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class AccountController : Controller
    {
        // GET: 登录页
        public ActionResult Login()
        {
            return View();
        }
        // Post: 登录
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            Session.Clear();

            System.Web.Security.FormsAuthentication.SetAuthCookie(username, true);
            System.Web.Security.FormsAuthentication.RedirectFromLoginPage(username, false);


            if (!(string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)))
            {
                string msg = string.Empty;
                bool result = false;

                var logintUsers = UserEntity.GetListByProperty(a => a.Name, username).SingleOrDefault();
                result = logintUsers.Password == password;

                if (result)
                {
                    System.Web.Security.FormsAuthentication.SetAuthCookie(username, true);
                    System.Web.Security.FormsAuthentication.RedirectFromLoginPage(username, false);
                    ViewBag.username = logintUsers.Name;
                    return RedirectToAction("Index", "Home");//


                }
                else
                {
                    ViewBag.errorMsg = "登录用户名或密码错误";
                    
                }
            }
            else
            {
                ViewBag.errorMsg = "用户名、密码不能为空！";
            }

            

            return View();
        }

        // GET: 登出
        public ActionResult Logout()
        {
            Session.Clear();
            System.Web.Security.FormsAuthentication.SignOut();

            return new RedirectResult("~/Account/Login");
        }

    }
}

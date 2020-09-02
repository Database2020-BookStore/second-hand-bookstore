using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        //登录界面
        public ActionResult Login()
        {
            return View();
        }
        //注册界面
        public ActionResult Regesiter()
        {
            return View();
        }
        //登录请求接口
        [HttpPost]
        public JsonResult LoginRequest(string username,string password)
        {
            List<Object> response=new List<Object>();
            if (username=="Ian"&&password=="123")
            {
                response.Add(new
                {
                    status="success",
                    uid=0
                });

            }
            else
            {
                response.Add(new
                {
                    status = "fail",
                    message="登录失败"
                });
            }
            return Json(response);
        }
        //注册请求接口
        [HttpPost]
        public JsonResult RegisterRequest(string username, string password,string email)
        {
            List<Object> response = new List<Object>();
            response.Add(new
            {
                status = "success"
            });
            return Json(response);
        }
    }
}
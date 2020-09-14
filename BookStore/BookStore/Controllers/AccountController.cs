using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBop;
using DateBaseTest;

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
        //个人中心
        public ActionResult Information()
        {
            return View();
        }

        //登录请求接口
        [HttpPost]
        public JsonResult LoginRequest(int user_id, string password)
        {
            DBops cc = new DBops();
            List<Object> response = new List<Object>();
            if (!cc.user_exist(user_id))
            {
                response.Add(new
                {
                    status = "fail",
                    message = "User doesn't exist"
                });
            }
            else
            {
                string pwd = cc.get_password(user_id);
                if (pwd != password)
                {
                    response.Add(new
                    {
                        status = "fail",
                        message = "Password is incorrect"
                    });
                }
                else
                {
                    USER user = cc.get_user_by_user_id(user_id);
                    string username = user.user_name;
                    response.Add(new
                    {
                        status = "success",
                        uid = user_id,
                        username = username
                    });
                }
            }
            return Json(response);
        }
        //注册请求接口
        [HttpPost]
        public JsonResult RegisterRequest(string username, int age, string password, string phone_number, string university, string department_name)
        {
            DBops cc = new DBops();
            List<Object> response = new List<Object>();
            int user_id = cc.generate_user_id();
            int dept_id = cc.get_dept_id_by_name(department_name);
            if (dept_id < 0)
            {
                response.Add(new
                {
                    status = "fail",
                    message = "department_name doesn't exist"
                });
            }
            else
            {
                cc.add_new_account(user_id, password, username, age, phone_number, university, dept_id);
                response.Add(new
                {
                    status = "success",
                    uid = user_id
                });
            }
            return Json(response);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////
        //获取用户信息
        [HttpPost]
        public JsonResult GetUserInformation(int user_id)
        {
            DBops cc = new DBops();
            List<Object> response = new List<Object>();
            USER user = cc.get_user_by_user_id(user_id);
            response.Add(new
            {
                status = "success",
                user_id = user_id,
                user_name = user.user_name,
                honesty = user.honesty,
                age = user.age,
                dept_id = user.dept_id,
                password = user.password,
                university = user.university,
                phone_number = user.phone_number
            });
            return Json(response);
        }

        //修改密码
        [HttpPost]
        public JsonResult ChangePassword(int user_id, string newPassword)
        {
            DBops cc = new DBops();
            List<Object> response = new List<Object>();
            cc.change_password(user_id, newPassword);
            response.Add(new
            {
                status = "success",
                uid = user_id
            });
            return Json(response);
        }
        //正在卖的书
        [HttpPost]
        public JsonResult GetSellingBooks(int user_id)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_solding_book();
            int num = 0;
            for (int i = 0; i < one.Count; i++)
            {
                if (one[i].price >= 0)
                {
                    num++;
                    string name = cc.get_book_name_from_good(one[i].good_id);
                    publishs.Add(new
                    {
                        goods_id = one[i].good_id,
                        book_id = one[i].book_id,
                        book_name = name,
                        price = one[i].price,
                        goods_description = one[i].good_description,
                        img = "Content/imgs/Books/1.png",
                    });

                }
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        //卖过的书
        [HttpPost]
        public JsonResult GetSelledBooks(int user_id)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_my_selled_goods(user_id);
            int num = 0;
            for (int i = 0; i < one.Count; i++)
            {
                if (one[i].price >= 0)
                {
                    num++;
                    string name = cc.get_book_name_from_good(one[i].good_id);
                    publishs.Add(new
                    {
                        goods_id = one[i].good_id,
                        book_id = one[i].book_id,
                        book_name = name,
                        price = one[i].price,
                        goods_description = one[i].good_description,
                        img = "Content/imgs/Books/1.png",
                    });

                }
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        //正在借的书
        [HttpPost]
        public JsonResult GetBorrowingBooks(int user_id)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_my_borrowed_goods(user_id);
            int num = 0;
            for (int i = 0; i < one.Count; i++)
            {
                if (one[i].price >= 0)
                {
                    num++;
                    string name = cc.get_book_name_from_good(one[i].good_id);
                    publishs.Add(new
                    {
                        goods_id = one[i].good_id,
                        book_id = one[i].book_id,
                        book_name = name,
                        price = one[i].price,
                        goods_description = one[i].good_description,
                        img = "Content/imgs/Books/1.png",
                    });

                }
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        //借过的书
        [HttpPost]
        public JsonResult GetBorrowedBooks(int user_id)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_my_borrowed_goods(user_id);
            int num = 0;
            for (int i = 0; i < one.Count; i++)
            {
                if (one[i].price >= 0)
                {
                    num++;
                    string name = cc.get_book_name_from_good(one[i].good_id);
                    publishs.Add(new
                    {
                        goods_id = one[i].good_id,
                        book_id = one[i].book_id,
                        book_name = name,
                        price = one[i].price,
                        goods_description = one[i].good_description,
                        img = "Content/imgs/Books/1.png",
                    });

                }
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        //买过
        [HttpPost]
        public JsonResult GetBoughtBooks(int user_id)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_my_bought_goods(user_id);
            int num = 0;
            for (int i = 0; i < one.Count; i++)
            {
                if (one[i].price >= 0)
                {
                    num++;
                    string name = cc.get_book_name_from_good(one[i].good_id);
                    publishs.Add(new
                    {
                        goods_id = one[i].good_id,
                        book_id = one[i].book_id,
                        book_name = name,
                        price = one[i].price,
                        goods_description = one[i].good_description,
                        img = "Content/imgs/Books/1.png",
                    });

                }
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }
    }
}
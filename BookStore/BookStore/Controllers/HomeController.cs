using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBop;
using DateBaseTest;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()     //首页
        {
            return View();
        }
        public ActionResult Mall()     //商城
        {
            return View();
        }
        int min(int a, int b)
        {
            return a > b ? b : a;
        }
        [HttpPost]
        public JsonResult getNewBorrowBooks(int number)     //获取展示在首页的最新上架的借出书籍
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_solding_book();
            one = one.OrderByDescending(o => o.good_id).ToList();//降序,id 就是时间
            int num = 0;
            for (int i = 0; i < min(one.Count, number); i++)
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
                if (num >= number) break;
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }


        [HttpPost]
        public JsonResult getNewSellBooks(int number)     //获取展示在首页的最新上架的售卖书籍
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops cc = new DBops();
            List<GOODS> one = cc.get_solding_book();
            one = one.OrderByDescending(o => o.good_id).ToList();//降序,id 就是时间
            int num = 0;
            for (int i = 0; i < min(one.Count, number); i++)
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
                if (num >= number) break;
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }
        [HttpPost]
        public JsonResult getRecommendBooks(int user_id)     //获取展示在首页的推荐书单
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            publishs.Add(new
            {
                goods_id = 100,
                user_id = 1,
                book_id = 100,
                book_name = "建筑工程制图(第6版)",
                price = 31,
                goods_description = "建筑工程制图(第6版) 9787560856711 同济大学出版社",
                img = "Content/imgs/Books/1.png",
            });
            publishs.Add(new
            {
                goods_id = 101,
                user_id = 2,
                book_id = 101,
                book_name = "工程结构",
                price = 29,
                goods_description = "工程结构（第三版） 9787560849768 同济大学出版社",
                img = "Content/imgs/Books/2.png",
            });
            publishs.Add(new
            {
                goods_id = 102,
                user_id = 3,
                book_id = 102,
                book_name = "数学实验",
                price = 16,
                goods_description = "数学实验（MATLAB版）（第4版 ） 韩明 同济大学出版社 9787560876030",
                img = "Content/imgs/Books/3.png",
            });
            publishs.Add(new
            {
                goods_id = 103,
                user_id = 4,
                book_id = 103,
                book_name = "概率论与数理统计",
                price = 16,
                goods_description = "概率论与数理统计同步习题册 9787560864686",
                img = "Content/imgs/Books/4.png",
            });
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }
    }
}
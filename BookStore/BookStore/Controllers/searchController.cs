using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DateBaseTest;
using DBop;

namespace yindan.Controllers
{
    public class searchController : Controller
    {
        // GET: search
        //检索页
        public ActionResult searchPage()
        {
            List<BOOK> books = new List<BOOK>();
            ViewData["user_id"] = 1;
            return View(books);
        }
        //显示检索结果
        public ActionResult fetch_list()
        {
            ViewData["user_id"] = Request["user_id"];
            int i;
            List<BOOK> books;
            string book_name = Request["book_name"];
            string course_name = Request["course_name"];
            if (book_name.Length == 0 && course_name.Length == 0)
            {
                ViewData["res"] = "检索关键字不为空！";
                books = new List<BOOK>();
            }
            else
            {
                
                if (course_name.Length == 0)  //仅根据书名检索
                {
                    DBops dbop = new DBops();
                    books = dbop.SearchBooksByName(book_name, book_name.Length);
                }
                else if (book_name.Length == 0)  //仅根据课程名检索
                    books = new DateBaseCmds().QueryBOOK("select * from book natural join course_dept natural join course where course_name='" + course_name + " '");
                else         //同时根据书名和课程名检索
                    books = new DateBaseCmds().QueryBOOK("select * from book natural join course_dept natural join course where book_name='" + book_name + " 'and course_name='" + course_name + "'");

                if (books.Count == 0)
                    ViewData["res"] = "未找到符合条件的条目";
                else
                {
                    //获取每个条目的category
                    for (i = 0; i < books.Count; i++)
                    {
                        string category = new DateBaseCmds().QueryString("select * from book where book_id='" + books[i].book_id + "'", "category");
                        books[i].describe = category;
                    }

                }
            }
            return View("searchPage", books);
        }



        //商品列表,可在此页面下单
        public ActionResult show_goods()
        {
            ViewData["user_id"] = Request["user_id"];
            ViewData["buyer_id"] = Request["user_id"];
            int book_id = int.Parse(Request["book_id"]);
            //检索所有该类别的商品
            List<GOODS> goods = new DateBaseCmds().QueryGOODS("select * from goods where book_id='" + book_id + "' and price>0");
            int i;
            //添加书名
            for (i = 0; i < goods.Count; i++)
                goods[i].book_name = Request["book_name"];
            //获取卖家id
            for (i = 0; i < goods.Count; i++)
                goods[i].saler_id = new DateBaseCmds().QueryInt("select * from publish where goods_id='" + goods[i].good_id + "'", "user_id");
            //获取总评分
            for (i = 0; i < goods.Count; i++)
                goods[i].score = new DateBaseCmds().QueryInt("select sum(purchase_score) as total_score from purchase where goods_id='" + goods[i].good_id + "'", "total_score");
            //按评分降序排序
            goods.Sort(new good_cmp<GOODS>());
            return View("show_goods", goods);
        }

        //跳转至下单确认页面
        public ActionResult place_order()
        {
            ViewData["goods_id"] = int.Parse(Request["goods_id"]);

            return View();
        }

        //下单
        public ActionResult place()
        {
            int goods_id = int.Parse(Request["goods_id"]);
            int saler_id = int.Parse(Request["saler_id"]);
            int buyer_id = int.Parse(Request["user_id"]);
            ViewData["book_name"] = Request["book_name"];
            ViewData["price"] = Request["price"];
            ViewData["res"] = "";
            int value = -1;
            new DateBaseCmds().test_connect();
            //插入购买记录到purchase表
            new DateBaseCmds().UpdateInsertDelete("insert into purchase (buyer_id,saler_id,goods_id,purchase_time,purchase_score,purchase_comment) values ('" + buyer_id + "','" + saler_id + "','" + goods_id + "','" + DateTime.Now.ToString() + "','-1','-1')");
            //将goods表z中相应条目的price设为-1
            new DateBaseCmds().UpdateInsertDelete("update goods set price='" + value + "' where goods_id='" + goods_id + "'");
            ViewData["res"] = "下单成功!";
            return View("place_order");
        }

        //查看可借阅品
        public ActionResult show_borrows()
        {
            ViewData["user_id"] = Request["user_id"];
            ViewData["buyer_id"] = Request["user_id"];
            int book_id = int.Parse(Request["book_id"]);
            //检索所有该类别的商品
            List<GOODS> goods = new DateBaseCmds().QueryGOODS("select * from goods where book_id='" + book_id + "' and price=-1");
            int i;
            //添加书名
            for (i = 0; i < goods.Count; i++)
                goods[i].book_name = Request["book_name"];
            //获取卖家id
            for (i = 0; i < goods.Count; i++)
                goods[i].saler_id = new DateBaseCmds().QueryInt("select * from publish where goods_id='" + goods[i].good_id + "'", "user_id");
            //获取总评分
            for (i = 0; i < goods.Count; i++)
                goods[i].score = new DateBaseCmds().QueryInt("select sum(purchase_score) as total_score from purchase where goods_id='" + goods[i].good_id + "'", "total_score");
            //按评分降序排序
            goods.Sort(new good_cmp<GOODS>());
            return View("show_borrows", goods);
        }

        //跳转至借阅页面
        public ActionResult borrow()
        {
            ViewData["goods_id"] = int.Parse(Request["goods_id"]);
            ViewData["saler_id"] = int.Parse(Request["saler_id"]);
            ViewData["user_id"] = int.Parse(Request["buyer_id"]);
            ViewData["book_name"] = Request["book_name"];
            return View("borrow");
        }

        //借阅
        public ActionResult add_to_borrow()
        {
            int goods_id = int.Parse(Request["goods_id"]);
            int saler_id = int.Parse(Request["saler_id"]);
            int buyer_id = int.Parse(Request["user_id"]);
            string due = Request["due"];
            ViewData["book_name"] = Request["book_name"];
            ViewData["price"] = Request["price"];
            ViewData["res"] = "";
            int value = -2;
            //插入购买记录到purchase表
            new DateBaseCmds().UpdateInsertDelete("insert into borrow (borrower_id,lender_id,goods_id,borrow_time,due) values ('" + buyer_id + "','" + saler_id + "','" + goods_id + "','" + DateTime.Now.ToString() + "','"+due+"')");
            //将goods表z中相应条目的price设为-2，表示已借出
            new DateBaseCmds().UpdateInsertDelete("update goods set price='" + value + "' where goods_id='" + goods_id + "'");
            ViewData["res"] = "借阅成功!";
            return View("borrow");
        }
    }


    //自定义GOODS类的比较,按评分降序排序
    class good_cmp<T> : IComparer<GOODS>
    {
        public int Compare(GOODS a, GOODS b)
        {
            if (a.score > b.score)
                return 1;
            return 0;
        }
    }


}
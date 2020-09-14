﻿using System;
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
        public ActionResult show_one_goods()     //单个商品页面
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
        int value(USER myself, BOOK one)//use user for save the time to connect database
        {
            DBops dBop = new DBops();
            int sorce = 0;
            List<USER> ans = dBop.buyer_id_list(one.book_id);
            for (int i = 0; i < ans.Count; i++)
            {
                if (ans[i].dept_id == myself.dept_id && ans[i].age == myself.age)
                {
                    sorce += 50;
                }
                else if (ans[i].dept_id == myself.dept_id)
                {
                    sorce += 20;
                }
                if (ans[i].user_id == myself.user_id)
                {// if you have bought this book ,of course you don't need it angin unless you lose it
                    sorce -= 5000;
                }
            }
            if (ans.Count<=0) return -1;
            sorce /= ans.Count;
            return sorce;

        }

        [HttpPost]
        public JsonResult getRecommendBooks(int user_id, int number)     //获取展示在首页的推荐书单
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();

            DBops dBop = new DBops();
            USER myself = dBop.get_user_by_user_id(user_id);
            List<BOOK> ans = dBop.get_all_book();//use book to anasys first


            for (int i = 0; i < ans.Count; i++)
            {
                ans[i].version = value(myself, ans[i]) * 100;//using this
            }

            ans.OrderByDescending(i => i.version);
            int num = 0;
            for (int i = 0; i < ans.Count; i++)
            {
                ans[i].version %= 100;
                List<GOODS> res = dBop.get_all_goods_from_book(ans[i].book_id);
                if (res.Count == 0) continue;
                GOODS one = res[0];
                publishs.Add(new
                {
                    goods_id = one.good_id,
                    user_id = user_id,
                    book_id = one.book_id,
                    book_name = dBop.get_book_name_from_good(one.good_id),
                    price = one.price,
                    goods_description = one.good_description,
                    img = "Content/imgs/Books/1.png",
                });
                num++;
                if (num == number) break;
            }
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        //获取所有符合条件的商品
        [HttpPost]
        public JsonResult getQuerryGoods(List<string> category, List<string> goodsType, int sort, string searchword, int page, int pageNum)
        {
            DBops cc = new DBops();
            List<Object> res = new List<Object>();
            List<Object> book_list = new List<Object>();
            List<BOOK> books;

            if (searchword == "")
            {
                books = cc.get_all_book();
            }
            else
            {
                books = cc.SearchBooksByName(searchword, 0);

            }

            List<int> goodsTypeIndex = new List<int>();
            for (int i = 0; i < goodsType.Count; i++)
            {
                if (goodsType[i] == "售卖")
                    goodsTypeIndex.Add(0);
                else if (goodsType[i] == "借出")
                    goodsTypeIndex.Add(1);
            }


            List<GOODS> goods = new List<GOODS>();
            for (int i = 0; i < books.Count; i++)
                goods = goods.Union(cc.get_goods_by_book_id(books[i].book_id)).ToList();
            List<GOODS> good_list = new List<GOODS>();
            for (int i = 0; i < goods.Count; i++)
            {
                if (goods[i].price < 0)
                    continue;
                bool ok = false;
                BOOK book = cc.get_book(goods[i].book_id);
                if (book == null) continue;

                if (category == null || category.Count == 0) ok = true;         //类型为空，所有种类的书籍
                else for (int j = 0; j < category.Count; j++)
                    {
                        if (book.category == category[j])
                        {
                            ok = true;
                            break;
                        }
                    }
                if (!ok)
                    continue;


                ok = false;
                int flag = 0;
                if (goods[i].price > 0)
                    flag = 0;
                else if (goods[i].price == 0)
                    flag = 1;
                for (int j = 0; j < goodsTypeIndex.Count; j++)
                {
                    if (flag == goodsTypeIndex[j])
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok)
                    good_list.Add(goods[i]);
            }




            List<Temp> temps = new List<Temp>();
            for (int i = 0; i < good_list.Count; i++)
            {
                Temp temp = new Temp();
                temp.good = good_list[i];
                temp.score = cc.get_average_score(good_list[i].book_id);
                temp.name = cc.get_book_name_from_good(good_list[i].good_id);
                temps.Add(temp);
            }


            switch (sort)
            {
                case 0:
                    temps = temps.OrderByDescending(o => o.good.good_id).ToList();
                    break;
                case 1:
                    temps = temps.OrderBy(o => o.good.good_id).ToList();
                    break;
                case 2:
                    temps = temps.OrderBy(o => o.good.price).ToList();
                    break;
                case 3:
                    temps = temps.OrderByDescending(o => o.good.price).ToList();
                    break;
                case 4:
                    temps = temps.OrderByDescending(o => o.name).ToList();
                    break;
                case 5:
                    temps = temps.OrderBy(o => o.name).ToList();
                    break;
            }


            for (int i = (page - 1) * pageNum; i < page * pageNum && i < temps.Count; i++)
            {
                GOODS good = temps[i].good;
                BOOK book = cc.get_book(temps[i].good.book_id);
                if (book == null) continue;



                PUBLISH publish = cc.get_publish_by_good_id(good.good_id);


                if (publish == null) continue;
                int uid = publish.user_id;
                book_list.Add(new
                {
                    goods_id = good.good_id,
                    user_id = uid,
                    book_id = good.book_id,
                    book_name = temps[i].name,
                    category = book.category,
                    price = good.price,
                    goods_description = good.good_description,
                    score=temps[i].score,
                    img = "Content/imgs/Books/1.png"
                });
            }
            res.Add(new
            {
                status = "success",
                bookList = book_list
            });
            return Json(res);
        }

        [HttpPost]
        public JsonResult getGoodsNum(List<string> category, List<string> goodsType, string searchword)   //获取符合条件的所有商品数量
        {


            DBops cc = new DBops();
            List<Object> res = new List<Object>();
            List<Object> book_list = new List<Object>();
            List<BOOK> books;

            if (searchword=="")
            {
                books = cc.get_all_book();
            }
            else
            {
                books = cc.SearchBooksByName(searchword, 0);

            }
            List<GOODS> goods = new List<GOODS>();


            for (int i = 0; i < books.Count; i++)
                goods = goods.Union(cc.get_goods_by_book_id(books[i].book_id)).ToList();
            List<GOODS> good_list = new List<GOODS>();

            List<int> goodsTypeIndex = new List<int>();
            for (int i = 0; i < goodsType.Count; i++)
            {
                if (goodsType[i] == "售卖")
                    goodsTypeIndex.Add(0);
                else if (goodsType[i] == "借出")
                    goodsTypeIndex.Add(1);
            }


            for (int i = 0; i < goods.Count; i++)
            {
                if (goods[i].price < 0)
                    continue;
                bool ok = false;
                BOOK book = cc.get_book(goods[i].book_id);
                if (book == null) continue;

                if (category==null||category.Count == 0) ok = true;         //类型为空，所有种类的书籍
                else for (int j = 0; j < category.Count; j++)
                {
                    if (book.category == category[j])
                    {
                        ok = true;
                        break;
                    }
                }
                if (!ok)
                    continue;


                ok = false;
                int flag = 0;
                if (goods[i].price > 0)
                    flag = 0;
                else if (goods[i].price == 0)
                    flag = 1;
                for (int j = 0; j < goodsTypeIndex.Count; j++)
                {
                    if (flag == goodsTypeIndex[j])
                    {
                        ok = true;
                        break;
                    }
                }
                if (ok)
                    good_list.Add(goods[i]);
            }

            List<Temp> temps = new List<Temp>();
            for (int i = 0; i < good_list.Count; i++)
            {
                Temp temp = new Temp();
                temp.good = good_list[i];
                temp.score = cc.get_average_score(good_list[i].book_id);
                temp.name = cc.get_book_name_from_good(good_list[i].good_id);
                temps.Add(temp);
            }
            int count = 0;
            for (int i = 0; i < temps.Count; i++)
            {
                GOODS good = temps[i].good;
                BOOK book = cc.get_book(temps[i].good.book_id);
                if (book == null) continue;

                PUBLISH publish = cc.get_publish_by_good_id(good.good_id);

                if (publish == null) continue;
                count++;
                
            }

            res.Add(new
            {
                status = "success",
                num = count
            });
            return Json(res);
        }

        [HttpPost]
        public JsonResult getCategory()      //获取所有书籍种类
        {
            DBops cc = new DBops();
            List<Object> response = new List<Object>();
            List<BOOK> arr = cc.get_all_book();
            arr = arr.OrderBy(o => o.category).ToList();
            List<string> categories = new List<string>();
            for (int i = 0; i < arr.Count; i++)
            {
                if (i == 0 || arr[i].category != arr[i - 1].category)
                {
                    categories.Add(arr[i].category);
                }
            }
            response.Add(new
            {
                status = "success",
                categories = categories
            });
            return Json(response);
        }

        [HttpPost]
        public JsonResult getGoodsInfo(int goods_id)
        {
            DBops cc = new DBops();
            List<Object> res = new List<Object>();
            GOODS good = cc.get_goods(goods_id);
            if (good == null)
            {
                res.Add(new
                {
                    status = "fail",
                    message = "Good doesn't exist"
                });
            }
            else
            {
                BOOK book = cc.get_book(good.book_id);
                int score = cc.get_average_score(good.book_id);
                List<BOOK_COMMENT> comments = cc.get_comment_by_book_id(good.book_id);
                List<Object> comment_list = new List<Object>();
                PUBLISH publish = cc.get_publish_by_good_id(goods_id);
                for (int i = 0; i < comments.Count; i++)
                {
                    USER user = cc.get_user_by_user_id(comments[i].user_id);
                    comment_list.Add(new
                    {
                        user_id = user.user_id,
                        user_name = user.user_name,
                        comment = comments[i].content,
                        //comment_time = comments[i].comment_time,
                        comment_score = comments[i].book_score
                    });
                }
                res.Add(new
                {
                    status = "success",
                    book_name = book.book_name,
                    price = good.price,
                    saler_id = publish.user_id,
                    gooods_description = good.good_description,
                    goods_score = score,
                    img_src = "Content/imgs/Books/1.png",
                    commentList = comment_list
                });
            }
            return Json(res);
        }
    }
}
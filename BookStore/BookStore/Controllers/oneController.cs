using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBop;
using DateBaseTest;

namespace BookStore.Controllers
{
    public class oneController : Controller
    {
        public ActionResult show_one_goods()     //单个商品页面
        {
            return View();
        }
        public ActionResult borrow_one_goods()     //单个借书页面
        {
            return View();
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
                if(book==null)
                {
                    res.Add(new
                    {
                        status = "fail",
                        message = "Good doesn't exist"
                    });
                    return Json(res);
                }
                int score = cc.get_average_score(good.book_id);
                List<BOOK_COMMENT> comments = cc.get_comment_by_book_id(good.book_id);
                List<Object> comment_list = new List<Object>();
                PUBLISH publish = cc.get_publish_by_good_id(goods_id);
                if (publish == null)
                {
                    res.Add(new
                    {
                        status = "fail",
                        message = "Good doesn't exist"
                    });
                    return Json(res);
                }
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
                    goods_description = good.good_description,
                    goods_score = score,
                    img_src = "Content/imgs/Books/1.png",
                    commentList = comment_list
                });
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult buy_goods(int buyer_id, int saler_id, int goods_id, string purchase_time, int purchase_score,
            string purchase_comment)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops one = new DBops();
            GOODS goods=one.get_goods(goods_id);
            one.insert_purchase(buyer_id, saler_id, goods_id, purchase_time, purchase_score,
             purchase_comment,goods.price);

            publishs.Add(new
            {
                buyer_id = buyer_id,
                saler_id = saler_id,
                goods_id = goods_id,
                purchase_time = purchase_time,
                purchase_score=purchase_score,
                purchase_comment= purchase_comment
            });
            response.Add(new
            {
                status = "success",
                bookList = publishs
            });
            return Json(response);
        }

        [HttpPost]
        public JsonResult borrow_goods(int borrower_id, int lender_id, int goods_id, string borrow_time, string due)
        {
            List<Object> response = new List<Object>();
            List<Object> publishs = new List<Object>();
            DBops one = new DBops();
            one.insert_borrow(borrower_id, lender_id, goods_id, borrow_time, due);

            response.Add(new
            {
                status = "success"
            });
            return Json(response);
        }
    }
}
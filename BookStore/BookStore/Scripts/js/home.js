﻿//数据
var Data = {
    isLogin: false,
    uid: 0,
    isLoading: [
        true, true, true,
        ],
    newBorrowBooks: [],
    newSellBooks: [],
    recommendBooks: [],
    recommendCard:[],
};

//方法
var Methods = {
    logout: function () {       //退出登录
        sessionStorage.removeItem('isLogin');
        sessionStorage.removeItem('uid');
        alert("已退出登录！");
        this.isLogin = false;
    },
    scrollDown:function(){      //向下滚动
        $('html,body').animate({scrollTop:$('#mainAnchor').offset().top}, 800);
    },
    getNewBorrowBooks: function () {
        url = location.origin + '/Home/getNewBorrowBooks';
        axios
            .post(url, { number:6 })
            .then((response)=> {
                if (response.status == 200) {
                    var data = response.data[0];
                    console.log(data);
                    if (data.status == 'success') {
                        for (var i = 0; i < data.bookList.length; i++) {
                            //data.bookList[i].img = location.origin + '/' + data.bookList[i].img;
                            data.bookList[i].img = location.origin + '/Content/imgs/Books/' + data.bookList[i].book_id + '.png';
                            data.bookList[i].href = location.origin + '/one/borrow_one_goods?goods_id='+data.bookList[i].goods_id;
                        }
                        this.newBorrowBooks = data.bookList;
                        console.log("Get New BorrowBooks!");
                        this.isLoading[0] = false;
                    }
                    else {
                        var message = data.message;
                        console.log(message);
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    },
    getNewSellBooks: function () {
        url = location.origin + '/Home/getNewSellBooks';
        axios
            .post(url, { number: 6 })
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        for (var i = 0; i < data.bookList.length; i++) {
                            //data.bookList[i].img = location.origin + '/' + data.bookList[i].img;
                            data.bookList[i].img = location.origin + '/Content/imgs/Books/' + data.bookList[i].book_id+ '.png';
                            data.bookList[i].href = location.origin + '/one/show_one_goods?goods_id=' + data.bookList[i].goods_id;
                        }
                        this.newSellBooks = data.bookList;
                        console.log("Get New SellBooks!");
                        this.isLoading[1] = false;
                    }
                    else {
                        var message = data.message;
                        console.log(message);
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    },
    getRecommendBooks: function () {
        url = location.origin +'/Home/getRecommendBooks';
        uid = this.uid;
        axios
            .post(url, { user_id: uid, number: 8})
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        for (var i = 0; i < data.bookList.length; i++) {
                            //data.bookList[i].img = location.origin + '/' + data.bookList[i].img;
                            data.bookList[i].img = location.origin + '/Content/imgs/Books/' + data.bookList[i].book_id+ '.png';
                            if (data.bookList[i].price > 0)
                                data.bookList[i].href = location.origin + '/one/show_one_goods?goods_id=' + data.bookList[i].goods_id;
                            else if (data.bookList[i].price == 0)
                                data.bookList[i].href = location.origin + '/one/borrow_one_goods?goods_id=' + data.bookList[i].goods_id;
                        }
                        this.recommendBooks= data.bookList;
                        console.log("Get RecommendBooks!");
                        this.getrecommendCard();
                    }
                    else {
                        var message = data.message;
                        console.log(message);
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    },
    getrecommendCard: function () {
        var rowNum = Math.ceil(this.recommendBooks.length / 4);
        var rows = [];
        for (var i = 0; i < rowNum; i++) {
            var row = this.recommendBooks.slice(i * 4, (i+ 1) * 4);
            rows.push(row);
        }
        this.recommendCard = rows;
        this.isLoading[2] = false;
    }
};

//创建时执行的函数
Created = function () {
    Data.isLogin = false;
    if (sessionStorage.getItem("isLogin")) {        //判断是否登录
        var uid = sessionStorage.getItem("uid");
        console.log("UID:" + uid);
        this.isLogin = true;
        this.uid = uid;
    }
    this.getNewBorrowBooks();
    this.getNewSellBooks();
    if (this.isLogin) {
        this.getRecommendBooks();
    }
    $('#navbar').removeClass('navbar-fixed');
}

//页面滚动导航栏变化
$(window).scroll(function () {
    var top = $(window).scrollTop();//获取body滚动距离
    if (top > 0) {
        $('#navbar').addClass('navbar-fixed');
    }
    else {
        $('#navbar').removeClass('navbar-fixed');
    }
});

//鼠标悬停在导航栏时变化
$("#navbar").hover(function () {
    $('#navbar').addClass('navbar-fixed');
}, function () {
    var top = $(window).scrollTop();//获取body滚动距离
    if (top == 0) {
        $('#navbar').removeClass('navbar-fixed');

    }
});
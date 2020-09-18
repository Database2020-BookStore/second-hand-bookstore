var Data = {
    isLogin: false,
    uid: 0,
    search: "",
    book_id: "",
    score: null,
    comment_description: "",
}
var Methods = {
    logout: function () {       //退出登录
        sessionStorage.removeItem('isLogin');
        sessionStorage.removeItem('uid');
        alert("已退出登录！");
        this.isLogin = false;
    },
    getUrlKey: function (name, url) {
        return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(url)
            || [, ""])[1].replace(/\+/g, '%20')) || null;
    },
    getBookID: function () {
        var href = window.location.href;
        var book_id = this.getUrlKey('bookid', href);
        if (book_id) {
            this.book_id = book_id;
        }
    },
    Publish_comment: function () {
        if (!this.isLogin) {
            alert("未登录！");
            return;
        }
        url = location.origin + '/Home/give_book_c';
        user_id = this.uid;
        book_id = this.book_id;
        score = this.score;
        price = this.price;
        comment_description = this.comment_description;
        axios
            .post(url, { user_id: user_id, book_id: book_id, score: score ,  comment_description: comment_description })
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {

                        console.log("Publish Success!");

                        alert("发布成功！");
                        location = '../';
                    }
                    else {
                        console.log("Publish Fail!");

                        alert("发布失败！");
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
        console.log("successs");
        console.log(score);
    }
}

Created = function () {
    Data.isLogin = false;
    if (sessionStorage.getItem("isLogin")) {        //判断是否登录
        var uid = sessionStorage.getItem("uid");
        console.log("UID:" + uid);
        this.isLogin = true;
        this.uid = uid;
    }
    this.getBookID();
}

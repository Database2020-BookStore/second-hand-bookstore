var Data = {
    isLogin: false,
    uid: 0,
    goodsID: -1,
    isBuy:false,
    imgSrc: "",
    userName: "",
    goodName: "",
    goodsDescription: "",
    price: 0.00,
    marks: 0.0,
    commentList: [],
};

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
    getGoodsID: function () {
        var href = window.location.href;
        var goods_id = this.getUrlKey('goods_id', href);
        if (goods_id) {
            this.goodsID = goods_id;
        }
    },
    getGoodsInfo: function () {
        url = location.origin + '/Home/getGoodsInfo';
        goodsID = this.goodsID;
        axios
            .post(url, { goods_id: goodsID })
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        this.price = data.price;
                        this.goodName = data.book_name; 3
                        this.goodsDescription = data.good_description;
                        this.imgSrc = data.img_src;
                        this.marks = data.goods_score;
                        this.commentList = data.commentList;
                        console.log(this.commentList);
                        console.log(data.commentList);
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
    checkLogin: function () {
        if (!this.isLogin) {
            alert('您还未登录，请点击导航栏“登录”按钮进行登录！');
        }
    },
};

Created = function () {
    Data.isLogin = false;
    if (sessionStorage.getItem("isLogin")) {        //判断是否登录
        var uid = sessionStorage.getItem("uid");
        console.log("UID:" + uid);
        this.isLogin = true;
        this.uid = uid;
    }
    this.getGoodsID();
    this.getGoodsInfo();
}
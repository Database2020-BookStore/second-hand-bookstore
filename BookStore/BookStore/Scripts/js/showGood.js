var Data = {
    isLogin: false,
    uid: 0,
    sid: 0,
    goodsID: 2,
    isBuy: false,
    imgSrc: "",
    userName: "",
    goodName: "",
    goodsDescription: "",
    price: 0.00,
    marks: 0.0,
    commentList: [],
    isLoading: true,
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
        url = location.origin + '/one/getGoodsInfo';
        var goodsID = this.goodsID;
        axios
            .post(url, { goods_id: goodsID })
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        this.price = data.price;
                        this.goodName = data.book_name;
                        this.goodsDescription = data.goods_description;
                        this.imgSrc = location.origin + '/Content/imgs/Books/' + data.book_id + '.png';
                        this.marks = data.goods_score;
                        this.commentList = data.commentList;
                        this.sid = data.saler_id;

                        this.isLoading = false;
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

    sendPurchaseInfo: function () {
        url = location.origin + '/one/buy_goods';
        var time = new Date();
        var get_time = time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate();
        var requestData = {
            buyer_id: this.uid, saler_id: this.sid, goods_id: this.goodsID,
            purchase_time: get_time, purchase_score: 0, purchase_comment: ""
        };
        axios
            .post(url, requestData)
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        alert("购买成功！订单已发送到您的个人信息页上！");
                        location = "../../"
                    }
                    else {
                        var message = data.message;
                        console.log(message);
                    }
                }
            })
    },

    confirm: function () {
        if (!this.isLogin) {
            alert('您还未登录，请点击导航栏“登录”按钮进行登录！');
            return;
        }
        else {
            var info = '“' + this.goodName + ' ￥' + this.price + '”' + '请确认购买信息。';
            this.$confirm(info, '确认购买', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'info'
            }).then(() => {
                this.$message({
                    type: 'success',
                    message: '购买成功!'
                });
                this.sendPurchaseInfo();
            }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消购买。'
                });
            });
        }
    }
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

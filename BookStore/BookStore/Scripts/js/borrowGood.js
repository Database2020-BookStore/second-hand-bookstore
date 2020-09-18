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
                        this.goodName = data.book_name;
                        this.goodsDescription = data.goods_description;
                        this.imgSrc = location.origin + '/Content/imgs/Books/' + data.book_id + '.png';
                        this.marks = data.goods_score;
                        this.commentList = data.commentList;
                        this.sid = data.saler_id;

                        this.isLoading = false;
                        console.log("Get Goods Info!");
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

    sendBorrowInfo: function () {
        url = location.origin + '/one/borrow_goods';
        var time = new Date();
        var get_time = time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate();
        var requestData = {
            borrower_id: this.uid, lender_id: this.sid, goods_id: this.goodsID,
            borrow_time: get_time, due: null
        };
        axios
            .post(url, requestData)
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    console.log(data);
                    if (data.status == 'success') {
                        alert("已成功向借书者发送借书请求，请耐心等待借书者的回应！");
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
            var info = '“' + this.goodName + '”' + '请确认借记信息。';
            this.$confirm(info, '确认借记信息', {
                confirmButtonText: '确定',
                cancelButtonText: '取消',
                type: 'info'
            }).then(() => {
                this.$message({
                    type: 'success',
                    message: '发送借记信息成功!'
                });
                this.sendBorrowInfo();
            }).catch(() => {
                this.$message({
                    type: 'info',
                    message: '已取消借记。'
                });
            });
        }
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
    console.log(sessionStorage.getItem("isLogin"));

    this.getGoodsID();
    this.getGoodsInfo();
}
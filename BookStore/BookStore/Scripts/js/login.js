var Data = {
    userid: "",
    password: "",
};

var Methods = {
    checkNumber: function (obj) {
        var reg = /^[0-9]+$/;
        if (obj != "" && !reg.test(obj)) {
            return false;
        }
        return true;
    },
    login: function () {            //登录
        url = location.origin+'/Account/LoginRequest';
        userid = this.userid;
        password = this.password;
        if (!this.checkNumber(userid)) {
            alert("账号格式错误！");
            return;
        }
        if (userid=="") {
            alert("账号不能为空！");
            return;
        }
        if (password == "") {
            alert("密码不能为空！");
            return;
        }
        axios
            .post(url, { user_id: userid , password: password })
            .then(function (response) {
                if (response.status == 200)
                {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        var uid = data.uid;
                        var username = data.username;
                        console.log("Login Success!");
                        sessionStorage.setItem("isLogin", true);
                        sessionStorage.setItem("uid", uid);
                        alert("登录成功！欢迎你，" + username +"！");
                        location = '../';
                    }
                    else
                    {
                        console.log("Login Fail!");
                        var message = data.message;
                        alert("登录失败！" + message);
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    }

};
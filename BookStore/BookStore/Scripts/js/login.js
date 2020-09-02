var Data = {
    username: "",
    password: "",
};

var Methods = {
    login: function () {            //登录
        url = location.origin+'/Account/LoginRequest';
        username = this.username;
        password = this.password;
        axios
            .post(url, { username: username , password: password })
            .then(function (response) {
                if (response.status == 200)
                {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        var uid = data.uid;
                        console.log("Login Success!");
                        sessionStorage.setItem("isLogin", true);
                        sessionStorage.setItem("uid", uid);
                        alert("登录成功！欢迎你，" + this.username+"！");
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
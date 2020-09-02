var Data = {
    username: "",
    email:"",
    password: "",
    repassword:"",
};

var Methods = {
    register: function () {              //注册
        url = location.origin +'/Account/RegisterRequest';
        axios
            .post(url, { username: this.username, password: this.password,email:this.email })
            .then(function (response) {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        console.log("Register!");
                        alert("注册成功！");
                        location = './Login';
                    }
                    else
                    {
                        console.log("Fail!");
                        alert("注册失败！");
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    }

};
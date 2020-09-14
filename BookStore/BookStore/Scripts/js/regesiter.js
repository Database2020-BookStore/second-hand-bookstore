var Data = {
    username: "",
    phone:"",
    password: "",
    repassword: "",
    age:"",
    school: "",
    department: "",
    regesiterSuccess: false,
    uid: ""
};

var Methods = {
    checkNumber: function(obj){
        var reg = /^[0-9]+$/;
        if (obj != "" && !reg.test(obj)) {
            return false;
        }
        return true;
    },
    chackForm: function () {
        var requestData = {
            username: this.username,
            phone: this.phone,
            password: this.password,
            age: this.age,
            school: this.school,
            department: this.department
        };
        for (var key in requestData) {
            if (requestData[key] == "") {
                alert("表单不完整，请重新填写！");
                return false;
            }
        }
        if (this.password != this.repassword) {
            alert("两次输入的密码不同，请重新填写！");
            return false;
        }
        if (this.password.length < 8) {
            alert("密码太短，请重新填写！");
            return false;
        }
        if (!this.checkNumber(this.phone)) {
            alert("手机号码有误，请重新填写！");
            return false;
        }
        if (!this.checkNumber(this.age)) {
            alert("年龄有误，请重新填写！");
            return false;
        }
        return true;
    },
    register: function () {              //注册
        url = location.origin + '/Account/RegisterRequest';
        if (!this.chackForm()) {
            return;
        }
        var requestData = {
            username: this.username,
            age: this.age,
            password: this.password,
            phone_number: this.phone,
            university: this.school,
            department_name: this.department
        };
        axios
            .post(url, requestData)
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        console.log("Register!");
                        this.regesiterSuccess = true;
                        this.uid = data.uid;
                        alert("注册成功！");
                    }
                    else
                    {
                        console.log("Fail!");
                        alert("注册失败！" + data.message);
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    },
    toLogin: function () {
        location = './Login';
    }

};
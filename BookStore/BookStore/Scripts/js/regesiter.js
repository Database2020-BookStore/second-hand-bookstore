var Data = {
    username: "",
    phone:"",
    password: "",
    repassword: "",
    age:"",
    school: "",
    department: "",
    regesiterSuccess: false,
    uid: "",
    options: [{
        value: '马克思主义学院',
        label: '马克思主义学院'
    }, {
        value: '数学科学学院',
        label: '数学科学学院'
    }, {
        value: '化学科学与工程学院',
        label: '化学科学与工程学院'
    }, {
        value: '土木工程学院',
        label: '土木工程学院'
    }, {
        value: '机械与能源工程学院',
        label: '机械与能源工程学院'
    }, {
        value: '环境科学与工程学院',
        label: '环境科学与工程学院'
    }, {
        value: '人文学院',
        label: '人文学院'
    }, {
        value: '材料科学与工程学院',
        label: '材料科学与工程学院'
    }, {
        value: '电子与信息工程学院',
        label: '电子与信息工程学院'
    }, {
        value: '外国语学院',
        label: '外国语学院'
    }, {
        value: '国际文化交流学院',
        label: '国际文化交流学院'
    }, {
        value: '医学院',
        label: '医学院'
    }, {
        value: '口腔医学院',
        label: '口腔医学院'
    }, {
        value: '交通运输工程学院',
        label: '交通运输工程学院'
    }, {
        value: '生命科学与技术学院',
        label: '生命科学与技术学院'
    }, {
        value: '汽车学院',
        label: '汽车学院'
    }, {
        value: '新生院',
        label: '新生院'
    }, {
        value: '海洋与地球科学学院',
        label: '海洋与地球科学学院'
    }, {
        value: '软件学院',
        label: '软件学院'
    }, {
        value: '铁道与城市轨道交通研究院',
        label: '铁道与城市轨道交通研究院'
    }, {
        value: '航空航天与力学学院',
        label: '航空航天与力学学院'
    }, {
        value: '中德工程学院',
        label: '中德工程学院'
    }, {
        value: '法学院',
        label: '法学院'
    }, {
        value: '政治与国际关系学院',
        label: '政治与国际关系学院'
    }, {
        value: '设计创意学院',
        label: '设计创意学院'
    }, {
        value: '测绘与地理信息学院',
        label: '测绘与地理信息学院'
    }, {
        value: '物理科学与工程学院',
        label: '物理科学与工程学院'
    }, {
        value: '艺术与传媒学院',
        label: '艺术与传媒学院'
    },],
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
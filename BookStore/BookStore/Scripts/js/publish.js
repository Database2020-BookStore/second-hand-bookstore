//数据
var Data = {
    isLogin: false,
    uid: 100,
    isSell: true,
    search: "",
    book_name: "",
    price: "",
    good_description: "",
    dept_name: "",
    category: "",
    publishing_house: "",
    version: "",
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


//方法
var Methods = {
    logout: function () {       //退出登录
        sessionStorage.removeItem('isLogin');
        sessionStorage.removeItem('uid');
        alert("已退出登录！");
        this.isLogin = false;
    },
    PublishBook: function () {
        url = location.origin + '/Home/PublishBook';
        user_id = this.user_id
        book_name = this.book_name;
        if (this.isSell) {
            price = -1;
        }
        else {
            price = this.price;
        }
        good_description = this.good_description;
        dept_name = this.dept_name;
        publishing_house = this.publishing_house;
        version = this.version;
        category = this.category;
        user_id = this.uid;
        console.log({ book_name: book_name, price: price, good_description: good_description, user_id: user_id, dept_name: dept_name, category: category, publishing_house: publishing_house, version: version });
        axios
            .post(url, { book_name: book_name, price: price, good_description: good_description, user_id:user_id, dept_name:dept_name, category:category, publishing_house: publishing_house, version: version })
           // .get(url, { book_name: book_name, price: price, good_description: good_description, dept_name: dept_name, category: category, publishing_house: publishing_house, version: version })

            .then(function (response) {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {

                        console.log(response);

                        alert("发布成功！");
                        location = '../';
                    }
                    else {
                        console.log("Publish Fail!");

                        alert("发布失败！请将书籍内容补充完整");
                    }
                }
            })
            .catch(function (error) { // 请求失败处理
                console.log(error);
            });
    }
}

//创建时执行的函数
Created = function () {
    Data.isLogin = false;
    if (sessionStorage.getItem("isLogin")) {        //判断是否登录
        var uid = sessionStorage.getItem("uid");
        console.log("UID:" + uid);
        this.isLogin = true;
        this.uid = uid;
    }
}

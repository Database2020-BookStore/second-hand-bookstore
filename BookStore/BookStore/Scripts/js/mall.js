//数据
var Data = {
    isLogin: false,
    uid: 0,
    isLoading: [true,true],
    search: "",
    bookCategory: [],
    searchword:[],
    selectbookCategory: [],
    selectType: ["借出", "售卖"],
    publishType: ["借出", "售卖"],
    currentSortType: 0,
    sortTypes: ["发布时间从晚到早", "发布时间从早到晚", 
        "价格从低到高", "价格从高到低", "名称从小到大","名称从大到小"],
    currentpageNum: 20,
    pageNums: [20, 40, 60],
    resultBooksNum: 0,
    showResult:'',
    currentPage: 1,
    allGoods:[],
    goodsCard: [],
};


//方法
var Methods = {
    logout: function () {       //退出登录
        sessionStorage.removeItem('isLogin');
        sessionStorage.removeItem('uid');
        alert("已退出登录！");
        this.isLogin = false;
    },
    selectTypeTagClose: function (tag) {
        this.selectType.splice(this.selectType.indexOf(tag), 1);
        this.refreshGoods();
    },
    bookCategoryTagClose: function (tag) {
        this.selectbookCategory.splice(this.selectbookCategory.indexOf(tag), 1);
        this.refreshGoods();
    },
    searchwordTagClose: function (tag) {
        this.searchword.splice(this.searchword.indexOf(tag), 1);
        this.refreshGoods();
    },
    selectTypeChange: function () {
        console.log(this.selectType);
        this.refreshGoods();
    },
    sortTypeChange: function (type) {
        this.currentSortType = this.sortTypes.indexOf(type);
        this.refreshGoods();
    },
    pageNumChange: function (pageNum) {
        this.currentpageNum = pageNum;
        this.refreshGoods();
    },
    pageChange: function () {
        console.log(this.currentPage);
        this.refreshGoods();
    },
    onSearchClick: function () {
        if (this.search != '') {
            this.searchword = [this.search];
            this.refreshGoods();
        }
    },
    onCategorySelect: function (index) {
        if (this.bookCategory[index]) {
            this.selectbookCategory = [this.bookCategory[index]];
            this.refreshGoods();
        }
    },
    refreshGoods: function () {
        this.cleanGoods();
        this.getResultNum();
        this.getGoods();
    },
    cleanGoods: function () {
        this.goodsCard = [];
        this.isLoading[0] = true;
    },
    getCategory: function () {
        url = location.origin + '/Home/getCategory';
        axios
            .post(url)
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        console.log("Get Category!");
                        this.bookCategory = data.categories;
                        this.isLoading[1] = false;
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
    getResultNum: function () {
        url = location.origin + '/Home/getGoodsNum';
        var searchword = "";
        if (this.searchword.length > 0) {
            searchword = this.searchword[0];

        }
        var requestData = {
            category: this.selectbookCategory, goodsType: this.selectType,searchword: searchword
        };
        console.log(requestData);
        axios
            .post(url, requestData)
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    console.log(data);

                    if (data.status == 'success') {
                        this.resultBooksNum = data.num;
                        console.log("Get Goods Num!");
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
    getGoods: function () {
        url = location.origin + '/Home/getQuerryGoods';
        var searchword = "";
        if (this.searchword.length > 0) {
            searchword = this.searchword[0];

        }
        var requestData = {
            category: this.selectbookCategory, goodsType: this.selectType, sort: this.currentSortType,
            searchword: searchword, page: this.currentPage, pageNum: this.currentpageNum
        };
        console.log(requestData);
        axios
            .post(url, requestData )
            .then((response) => {
                if (response.status == 200) {
                    var data = response.data[0];
                    if (data.status == 'success') {
                        for (var i = 0; i < data.bookList.length; i++) {
                            data.bookList[i].img = location.origin + '/' + data.bookList[i].img;
                        }
                        this.allGoods = data.bookList;
                        console.log("Get Goods!");
                        this.getGoodsCard();
                        this.isLoading[0]= false;
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
    getGoodsCard: function () {
        var rowNum = Math.ceil(this.allGoods.length / 4);
        var rows = [];
        for (var i = 0; i < rowNum; i++) {
            var row = this.allGoods.slice(i * 4, (i + 1) * 4);
            rows.push(row);
        }
        this.goodsCard = rows;

    },
    getUrlKey: function (name, url) {
        return decodeURIComponent((new RegExp('[?|&]' + name + '=' + '([^&;]+?)(&|#|;|$)').exec(url)
            || [, ""])[1].replace(/\+/g, '%20')) || null;

    },
    getArgs: function () {
        var href = window.location.href;
        var page = this.getUrlKey('page', href);
        var category = this.getUrlKey('category', href);
        var searchword = this.getUrlKey('search', href);
        var sortTypes = this.getUrlKey('sort', href);
        var pageNum = this.getUrlKey('num', href);
        if (page) {
            console.log(page);
            this.currentPage = page;
        }
        if (category) {
            console.log(category);
            this.selectbookCategory.push(category);
        }
        if (searchword) {
            console.log(searchword);
            this.searchword = [searchword];
        }
        if (sortTypes) {
            console.log(sortTypes);
            this.currentSortType = sortTypes;
        }
        if (pageNum) {
            console.log(pageNum);
            this.currentpageNum = pageNum;
        }
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
    this.getArgs();
    this.getCategory();
    this.getResultNum();
    this.getGoods();
}


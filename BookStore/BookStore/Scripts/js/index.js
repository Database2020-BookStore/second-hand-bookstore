//数据
if (typeof Data == "undefined") {
    window.Data = {};

}

//方法
if (typeof Methods == "undefined") {
    window.Methods = {};
}


//创建时执行的函数
if (typeof Created == "undefined") {
    window.Created = function () { };
}

$('#navbar').addClass('navbar-fixed');


var app = new Vue({
    el: '#main',
    data: Data,
    methods: Methods,
    created: Created,
});


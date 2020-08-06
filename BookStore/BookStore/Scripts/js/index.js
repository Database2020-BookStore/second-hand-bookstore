const isNullOrUndefined = obj => obj === null || obj === undefined;
if (typeof Data == "undefined") {
    window.Data = {};

}
if (typeof Methods == "undefined") {
    window.Methods = {};
}

var app = new Vue({
    el: '#main',
    data: Data,
    methods: Methods
});


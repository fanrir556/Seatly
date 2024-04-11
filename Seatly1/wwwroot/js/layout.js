// 獲取cookie驗證活動方是否登入
$.ajax({
    url: '/api/Organizers/cookie', // 后端 API 的 URL
    type: 'GET',
    success: function (response) {
        // 成功获取到响应数据后，处理数据
        console.log(response);
    },
    error: function (err) {
        // 处理请求失败的情况
        console.log(err.responseText);
    }
});
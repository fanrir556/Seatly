$.ajax({
    url: '/api/Organizers/cookie', // 后端 API 的 URL
    type: 'GET',
    success: function (data) {

        organizerid = JSON.stringify(data);
        console.log(`登入的活動方Id是${organizerid}`);

         //取得活動方的帳號與名稱並在Navbar顯示
        $.ajax({
            url: `/api/Organizers/account/${organizerid}`, // 后端 API 的 URL
            type: 'GET',
            success: function (data2) {

                console.log(`登入的活動方帳號是${data2}`);

            },
            error: function (err) {
                console.log(err.responseText);
            }
        });
    },
    error: function (err) {
        console.log(err.responseText);
    }
});

$('#o-logout').click(function () {
    $.ajax({
        url: '/api/Organizers/logout',
        type: 'POST',
        success: function (response) {
            // 处理成功响应
            console.log(response);
            // 刷新页面
            location.reload();
        },
        error: function (err) {
            // 处理错误响应
            console.error(err.responseText);
        }
    });
});
﻿// 取得登入的活動方id
$.ajax({
    url: '/api/OrganizersApi/cookie', // 后端 API 的 URL
    type: 'GET',
    success: function (data) {
        $("#OrganizersLoginRegisterNav").hide();
        $("#OrganizersManageNav").show();

        // 取出存在cookie當中的活動方id存到session
        sessionStorage.setItem("OrganizerId", data);
    },
    error: function (err) {
        $("#OrganizersLoginRegisterNav").show();
        $("#OrganizersManageNav").hide();
        console.log(err.responseText)
    }
});

// 登出控制
$('#o-logout').click(function () {
    $('#logoutModal').modal('show');
});

$('#logoutModalBtn2').click(function () {
    $('#logoutModal').modal('hide');
});

$('#closelogoutModalBtn').click(function () {
    $.ajax({
        url: '/api/OrganizersApi/logout',
        type: 'POST',
    }).done(function () {
        $('#logoutModal').modal('hide'); // 關閉模態框
        sessionStorage.clear();
        // 登出成功，重定向到首页
        window.location.href = '/';

        // 隐藏下拉菜单
        $("#OrganizersLoginRegisterNav").show();
        $("#OrganizersManageNav").hide();

    }).fail(function (err) {
        // 登录失败，显示错误消息
        $('#errorMessage').text(err.responseText);
        $('#loginErrorModal').modal('show');
    })
});
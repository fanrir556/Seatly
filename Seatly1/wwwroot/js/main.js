// 獲取cookie驗證活動方是否登入
//$.ajax({
//    url: '/api/Organizers/cookie', // 后端 API 的 URL
//    type: 'GET',
//    success: function (response) {
//        // 成功获取到响应数据后，处理数据
//        alert(response);
//    },
//    error: function (textStatus, errorThrown) {
//        // 处理请求失败的情况
//        alert('活動方未登入', textStatus, errorThrown);
//    }
//});


// 關閉登入失敗Modal
$('#closeErrorModalBtn').click(function () {
    $('#loginErrorModal').modal('hide'); // 關閉模態框
});

$('#closeErrorModalBtn2').click(function () {
    $('#loginErrorModal').modal('hide'); // 關閉模態框
});

// 登录表单提交时的事件处理程序
$('#loginForm').submit(function (event) {
    event.preventDefault(); // 阻止表单默认提交行為

    // 获取用户输入的账号和密码
    var loginaccount = $('#loginaccount').val();
    var loginpassword = $('#loginpassword').val();

    if (loginaccount == "" && loginpassword == "") {
        $('#loginaccount').addClass("is-invalid");
        $('#loginpassword').addClass("is-invalid");
    }
    else if (loginaccount == "") {
        $('#loginaccount').addClass("is-invalid");
    }
    else if (loginpassword == "") {
        $('#loginpassword').addClass("is-invalid");
    }
    else if (loginaccount != "" && loginpassword != "") {
        $('#loginaccount').addClass("is-valid");
        $('#loginpassword').addClass("is-valid");

        // 发送Ajax请求到后端
        $.ajax({
            url: '/api/Organizers/login', // 后端登录接口的URL
            type: 'POST',
            data: JSON.stringify({
                OrganizerAccount: loginaccount,
                LoginPassword: loginpassword
            }),
            contentType: 'application/json'
        }).done(function (result) {
            alert("登入成功", result);
            // 登录成功，重定向到首页
            window.location.href = '/';
        }).fail(function (err) {
            // 登录失败，显示错误消息
            $('#errorMessage').text(err.responseText);
            $('#loginErrorModal').modal('show');
        })
    }
});

$('#registerForm').submit(function (event) {
    event.preventDefault(); // 阻止表单默认提交行為

    // 获取用户输入的註冊表單資訊
    let registeraccount = $('#account').val();
    let name = $('#name').val();
    let category = $('#category').val();
    let photo = "example.png";
    let menu = $('#menu').val();
    let address = $('#address').val();
    let url = $('#URL').val();
    let hashtag = $('#hashtag').val();
    let email = $('#email').val();
    let phone = $('#phone').val();
    let registerpassword1 = $('#password1').val();
    let registerpassword2 = $('#password2').val();
    let validation = false;

    if (registerpassword1 != registerpassword2) {
        $('#password1').addClass("is-invalid");
        $('#invalid_password1').text("密碼與確認密碼欄位不一致");
        $('#password2').addClass("is-invalid");
        $('#invalid_password2').text("密碼與確認密碼欄位不一致");
    }
    else if (registerpassword1 == "") {
        $('#invalid_password1').text("密碼不可為空");
    }
    else if (registerpassword2 == "") {
        $('#invalid_password2').text("密碼不可為空");
    }
    else {// 发送Ajax请求到后端
        $.ajax({
            url: '/api/Organizers/register', // 后端登录接口的URL
            type: 'POST',
            data: JSON.stringify({
                OrganizerAccount: registeraccount,
                LoginPassword: registerpassword1,
                OrganizerName: name,
                OrganizerCategory: category,
                OrganizerPhoto: photo,
                Menu: menu,
                Address: address,
                ReservationUrl: url,
                Hashtag: hashtag,
                Email: email,
                Phone: phone,
                Validation: validation
            }),
            contentType: 'application/json'
        }).done(function (result) {
            alert("註冊成功", result);
            // 登录成功，重定向到首页
            window.location.href = '/';
        }).fail(function (err) {
            alert(err.responseText);
            //$('#errorMessage').text(err.StatusText);
            //$('#loginErrorModal').modal('show');
        })
    }

});

// 上傳活動方照片
$(document).ready(function () {
    $('#uploadButton').click(function () {
        var formData = new FormData();
        var fileInput = document.getElementById('imageInput');
        var file = fileInput.files[0];
        if (!file) {
            alert('Please select a file.');
            return;
        }
        formData.append('image', file);

        $.ajax({
            url: '/api/Organizers/uploads', // 后端上传图片的 URL
            type: 'POST',
            data: formData,
            processData: false, // 告诉 jQuery 不要处理数据
            contentType: false, // 告诉 jQuery 不要设置 contentType
            success: function (response) {
                $('#status').text('Image uploaded successfully.');
                console.log('Image uploaded successfully:', response);
            },
            error: function (textStatus, errorThrown) {
                $('#status').text('Error uploading image.');
                console.error('Error uploading image:', textStatus, errorThrown);
            }
        });
    });
});

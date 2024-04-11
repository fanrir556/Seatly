// 關閉登入失敗Modal
$('#closeErrorModalBtn').click(function () {
    $('#loginErrorModal').modal('hide'); // 關閉Modal
});

$('#closeErrorModalBtn2').click(function () {
    $('#loginErrorModal').modal('hide'); // 關閉Modal
});

// 活動方登入程式
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

// 活動方註冊程式
$('#registerForm').submit(function (event) {
    event.preventDefault(); // 阻止表单默认提交行为

    // 获取用户输入的注册表单信息
    let registeraccount = $('#account').val();
    let name = $('#name').val();
    let category = $('#category').val();
    let photo; // 声明photo变量
    let menu = $('#menu').val();
    let address = $('#address').val();
    let url = $('#URL').val();
    let hashtag = $('#hashtag').val();
    let email = $('#email').val();
    let phone = $('#phone').val();
    let registerpassword1 = $('#password1').val();
    let registerpassword2 = $('#password2').val();
    let validation = false;
    let formData = new FormData();
    let fileInput = document.getElementById('imageInput');
    let file = fileInput.files[0];
    
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
    else if (!file) {
        $('#imageInput').addClass("is-invalid");
        $('#invalid_imageInput').text("請選擇要上傳的圖片");
        return;
    }
    else {
        // 获取当前日期和时间
        var currentDate = new Date();
        var dateString = currentDate.getFullYear() + ('0' + (currentDate.getMonth() + 1)).slice(-2) + ('0' + currentDate.getDate()).slice(-2);
        var timeString = ('0' + currentDate.getHours()).slice(-2) + ('0' + currentDate.getMinutes()).slice(-2) + ('0' + currentDate.getSeconds()).slice(-2);
        var fileExtension = file.name.split('.').pop(); // 获取文件的扩展名
        var fileName = dateString + '_' + timeString + '.' + fileExtension; // 文件名只保留附檔名，其他部分用日期和时间替换

        formData.append('image', file, fileName);

        // 将文件名存储到photo变量中
        photo = fileName;

        // 发送Ajax请求到后端上传图片
        $.ajax({
            url: '/api/Organizers/uploads', // 后端上传图片的 URL
            type: 'POST',
            data: formData,
            processData: false, // 告诉 jQuery 不要处理数据
            contentType: false, // 告诉 jQuery 不要设置 contentType
            success: function (response) {
                $('#status').text('Image uploaded successfully.');
                console.log('Image uploaded successfully:', response);

                // 图片上传成功后，再发送表单数据
                $.ajax({
                    url: '/api/Organizers/register', // 后端登录接口的 URL
                    type: 'POST',
                    data: JSON.stringify({
                        OrganizerAccount: registeraccount,
                        LoginPassword: registerpassword1,
                        OrganizerName: name,
                        OrganizerCategory: category,
                        OrganizerPhoto: photo, // 使用新的文件名
                        Menu: menu,
                        Address: address,
                        ReservationUrl: url,
                        Hashtag: hashtag,
                        Email: email,
                        Phone: phone,
                        Validation: validation
                    }),
                    contentType: 'application/json',
                    success: function (result) {
                        alert(result);
                        // 登录成功，重定向到首页
                        window.location.href = '/';
                    },
                    error: function (err) {
                        alert(err.responseText);
                    }
                });
            },
            error: function (textStatus, errorThrown) {
                $('#status').text('Error uploading image.');
                console.error('Error uploading image:', textStatus, errorThrown);
            }
        });
    }
});
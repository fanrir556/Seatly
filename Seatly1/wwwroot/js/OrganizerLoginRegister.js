/*參考、引用資料：
1. 忘記密碼：https://blog.hungwin.com.tw/aspnet-mvc-member-forget-reset-pwd/
*/

/* 預覽上傳圖片 */

// 引用自：https://codepen.io/tohousanae/pen/mdgzYxZ
$(function () {
    $('#imageInput').on('change', function (event) {
        var files = event.target.files;
        var image = files[0]
        var reader = new FileReader();
        reader.onload = function (file) {
            var img = new Image();
            console.log(file);
            img.src = file.target.result;
            $('#preview').attr('src', img.src);
        }
        reader.readAsDataURL(image);
        console.log(files);
    });
});

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
    var account = $('#loginaccount').val();
    var password = $('#loginpassword').val();

        // 发送Ajax请求到后端
        $.ajax({
            url: '/api/OrganizersApi/login', // 后端登录接口的URL
            type: 'POST',
            data: JSON.stringify({
                OrganizerAccount: account,
                LoginPassword: password
            }),
            contentType: 'application/json'
        }).done(function (result) {
            alert("登入成功", result);
            // 登录成功，重定向到首页
            window.location.href = '/OrganizerRoute/Index';
        }).fail(function (err) {
            // 登录失败，显示错误消息
            $('#errorMessage').text(err.responseText);
            $('#loginErrorModal').modal('show');
        })
});

// 活動方註冊
$('#registerForm').submit(function (event) {
    event.preventDefault(); // 阻止表单默认提交行为

    // 获取用户输入的注册表单信息
    let account = $('#account').val();
    let name = $('#name').val();
    let category = $('#category').val();
    let photo;
    let menu = $('#menu').val();
    let address = $('#address').val();
    let url = $('#URL').val();
    let hashtag = $('#hashtag').val();
    let email = $('#email').val();
    let phone = $('#phone').val();
    let password1 = $('#password1').val();
    let password2 = $('#password2').val();
    let validation = false;
    let formData = new FormData();
    let fileInput = document.getElementById('imageInput');
    let file = fileInput.files[0];

    // 密碼與確認密碼欄位一致
    if (password1 != password2) {
        $('#password1').removeClass("is-valid");
        $('#password1').addClass("is-invalid");
        $('#invalid_password1').text("密碼與確認密碼欄位不一致");
        $('#password2').removeClass("is-valid");
        $('#password2').addClass("is-invalid");
        $('#invalid_password2').text("密碼與確認密碼欄位不一致");
    }

    // 電子郵件格式
    else if ((/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/).test(email) == false) {
        $('#email').removeClass("is-valid");
        $('#email').addClass("is-invalid");
        $('#invalid_email').text("email格式不正確");
    }

    // 網址格式
    else if ((/^(http|https):\/\/[^\s$.?#].[^\s]*$/).test(url) == false) {
        $('#URL').removeClass("is-valid");
        $('#URL').addClass("is-invalid");
        $('#invalid_url').text("網址格式不正確");
    }

    /*高強度密碼
    至少包含一個數字
    至少包含一個大寫字母
    至少包含一個小寫字母
    至少包含一個特殊字符 (不包含底線、空白、冒號)
    長度在8到16個字符之間
    */
    else if ((/^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$/).test(password1) == false) {
        console.log("密碼至少包含一個數字、一個大寫字母、至少包含一個小寫字母、至少包含一個特殊字符(不包含底線、空白、冒號)、長度在8到16個字符之間");
        $('#password1').removeClass("is-valid");
        $('#password1').addClass("is-invalid");
        $('#invalid_password1').text("密碼至少包含一個數字、一個大寫字母、至少包含一個小寫字母、至少包含一個特殊字符(不包含底線、空白、冒號)、長度在8到16個字符之間");
    }

    else {
        $('#password1').addClass("is-valid");
        $('#password2').addClass("is-valid");
        $('#email').addClass("is-valid");
        $('#URL').addClass("is-valid");

        // 获取当前日期和时间
        var currentDate = new Date();
        var dateString = currentDate.getFullYear() + ('0' + (currentDate.getMonth() + 1)).slice(-2) + ('0' + currentDate.getDate()).slice(-2);
        var timeString = ('0' + currentDate.getHours()).slice(-2) + ('0' + currentDate.getMinutes()).slice(-2) + ('0' + currentDate.getSeconds()).slice(-2);
        var fileExtension = file.name.split('.').pop(); // 获取文件的扩展名
        var fileName = dateString + '_' + timeString + '.' + fileExtension; // 文件名只保留附檔名，其他部分用日期和时间替换

        formData.append('image', file, fileName);

        // 将文件名存储到photo变量中
        photo = fileName;

        // 使用axios发送请求到后端上传图片
        axios({
            url: '/api/OrganizersApi/uploads', // 后端上传图片的 URL
            method: 'POST',
            data: formData,
            headers: {
                'Content-Type': 'multipart/form-data'
            }
        }).then(function (response) {
            $('#status').text('圖片上傳成功');
            console.log('Image uploaded successfully:', response);

            // 图片上传成功后，再发送表单数据
            return axios({
                url: '/api/OrganizersApi/register', // 后端登录接口的 URL
                method: 'POST',
                data: {
                    OrganizerAccount: account,
                    LoginPassword: password1,
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
                },
                headers: {
                    'Content-Type': 'application/json'
                }
            });
        }).then(function (result) {
            alert(result.data);
            // 註冊成功，重定向到首頁
            window.location.href = '/';
        }).catch(function (error) {
            if (error.response) {
                alert(error.response.data);
            } else if (error.request) {
                $('#status').text('Error uploading image.');
                console.error('Error uploading image:', error.request);
            } else {
                console.error('Error', error.message);
            }
        });

    }
});

// 忘記密碼前端部分
//$('#forgotPasswordForm').click(function () {

//    var username = $('#username').val();

//    $.ajax({
//        type: 'POST',
//        url: '/api/ForgotPassword',
//        data: { username: username },
//        success: function (response) {
//            $('#result').html(response);
//            console.log("密碼已寄送到您的註冊信箱");
//        },
//        error: function () {
//            $('#result').html('發生錯誤，請稍後再試。');
//        }
//    });
//});

// Example starter JavaScript for disabling form submissions if there are invalid fields
// Wait for the DOM to be ready
document.addEventListener("DOMContentLoaded", function () {

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation');

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault();
                    event.stopPropagation();
                }

                form.classList.add('was-validated');
            }, false);
        });
});
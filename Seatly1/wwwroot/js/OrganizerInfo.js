// 顯示活動方資訊
//$.ajax({
//    url: `/api/OrganizersApi/info/${organizerid}`, // 后端 API 的 URL
//    type: 'GET',
//    success: function (organizerinfo) {
//        console.log(organizerinfo);
//        $('#account').val(organizerinfo.organizerAccount);
//        $('#name').val(organizerinfo.organizerName);
//        $('#URL').val(organizerinfo.reservationUrl);
//        $('#email').val(organizerinfo.email);
//        $('#phone').val(organizerinfo.phone);

//        // 取得預覽圖片
//        // 引用程式碼：https://www.dotblogs.com.tw/supershowwei/2022/06/20/download-binary-data-like-image-using-jquery-ajax
//        let photoName = encodeURI(`FileName=${organizerinfo.organizerPhoto}`); // 預覽圖片名稱 
//        $.ajax({
//            url: `/api/OrganizersApi/photo/${organizerid}?${photoName}`, // 后端 API 的 URL
//            type: 'GET',
//            xhrFields: { responseType: 'blob' }, // 回傳圖片為blob物件
//            success: function (data) {
//                console.log(`圖片轉換成功${data}`);

//                // blob物件轉換成圖片
//                const fileReader = new FileReader();

//                fileReader.onload = e => {
//                    $('#preview').attr('src', e.target.result).show();
//                    $('#imageInput').attr('src', e.target.result);
//                };
//                fileReader.readAsDataURL(data);
//            },
//            error: function (err) {
//                console.log(`圖片轉換失敗${err.responseText}`);
//            }
//        });
//    },
//    error: function (err) {
//        console.log(err.responseText)
//    }
//});

// 修改活動方資訊
//$('#modifyOrganizerForm').submit(function (event) {
//    event.preventDefault(); // 阻止表单默认提交行为

//    // 获取用户输入的注册表单信息
//    let name = $('#name').val();
//    let photo;
//    let url = $('#URL').val();
//    let email = $('#email').val();
//    let phone = $('#phone').val();
//    let password1 = $('#password1').val();
//    let password2 = $('#password2').val();
//    let formData = new FormData();
//    let fileInput = document.getElementById('imageInput');
//    let file = fileInput.files[0];

//    // 密碼與確認密碼欄位一致
//    if ((password1 != password2) && (password1 || password2) != '') {
//        $('#password1').removeClass("is-valid");
//        $('#password1').addClass("is-invalid");
//        $('#invalid_password1').text("密碼與確認密碼欄位不一致");
//        $('#password2').removeClass("is-valid");
//        $('#password2').addClass("is-invalid");
//        $('#invalid_password2').text("密碼與確認密碼欄位不一致");
//    }

//    // 電子郵件格式
//    else if ((/^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/).test(email) == false && (email != '')) {
//        $('#email').removeClass("is-valid");
//        $('#email').addClass("is-invalid");
//        $('#invalid_email').text("email格式不正確");
//    }

//    // 網址格式
//    else if ((/^(http|https):\/\/[^\s$.?#].[^\s]*$/).test(url) == false && (url != '')) {
//        $('#URL').removeClass("is-valid");
//        $('#URL').addClass("is-invalid");
//        $('#invalid_url').text("網址格式不正確");
//    }

//    /*高強度密碼
//    至少包含一個數字
//    至少包含一個大寫字母
//    至少包含一個小寫字母
//    至少包含一個特殊字符 (不包含底線、空白、冒號)
//    長度在8到16個字符之間
//    */
//    else if ((/^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,16}$/).test(password1) == false && (password1 || password2) != '') {
//        console.log("密碼至少包含一個數字、一個大寫字母、至少包含一個小寫字母、至少包含一個特殊字符(不包含底線、空白、冒號)、長度在8到16個字符之間");
//        $('#password1').removeClass("is-valid");
//        $('#password1').addClass("is-invalid");
//        $('#invalid_password1').text("密碼至少包含一個數字、一個大寫字母、至少包含一個小寫字母、至少包含一個特殊字符(不包含底線、空白、冒號)、長度在8到16個字符之間");
//    }
//    else {
//        $('#password1').addClass("is-valid");
//        $('#password2').addClass("is-valid");
//        $('#email').addClass("is-valid");
//        $('#URL').addClass("is-valid");

//        if (file == null) {
//            // 透過Session取得活動方的id
//            const organizerid = sessionStorage.getItem("OrganizerId");
//            console.log(`Session：${organizerid}`);

//            $.ajax({
//                url: `/api/OrganizersApi/put/${organizerid}`,
//                type: 'PUT',
//                data: JSON.stringify({
//                    LoginPassword: password1,
//                    OrganizerName: name,
//                    ReservationUrl: url,
//                    Email: email,
//                    Phone: phone,
//                }),
//                contentType: 'application/json',
//                success: function (result) {
//                    alert(result);
//                    // 修改成功
//                    window.location.href = '/OrganizerRoute/Index';
//                },
//                error: function (err) {
//                    alert(err.responseText);
//                }
//            });
//        }
//        else {
//            // 获取当前日期和时间
//            var currentDate = new Date();
//            var dateString = currentDate.getFullYear() + ('0' + (currentDate.getMonth() + 1)).slice(-2) + ('0' + currentDate.getDate()).slice(-2);
//            var timeString = ('0' + currentDate.getHours()).slice(-2) + ('0' + currentDate.getMinutes()).slice(-2) + ('0' + currentDate.getSeconds()).slice(-2);
//            var fileExtension = file.name.split('.').pop(); // 获取文件的扩展名
//            var fileName = dateString + '_' + timeString + '.' + fileExtension; // 文件名只保留附檔名，其他部分用日期和时间替换

//            formData.append('image', file, fileName);

//            // 将文件名存储到photo变量中
//            photo = fileName;

//            // 发送Ajax请求到后端上传图片
//            $.ajax({
//                url: '/api/OrganizersApi/uploads', // 后端上传图片的 URL
//                type: 'POST',
//                data: formData,
//                processData: false, // 告诉 jQuery 不要处理数据
//                contentType: false, // 告诉 jQuery 不要设置 contentType
//                success: function (response) {
//                    $('#status').text('Image uploaded successfully.');
//                    console.log('Image uploaded successfully:', response);

//                    // 透過Session取得活動方的id
//                    const organizerid = sessionStorage.getItem("OrganizerId");
//                    console.log(`Session：${organizerid}`);

//                    // 图片上传成功后，再发送表单数据
//                    $.ajax({
//                        url: `/api/OrganizersApi/put/${organizerid}`,
//                        type: 'PUT',
//                        data: JSON.stringify({
//                            OrganizerPhoto: photo, // 使用新的文件名
//                            LoginPassword: password1,
//                            OrganizerName: name,
//                            ReservationUrl: url,
//                            Email: email,
//                            Phone: phone,
//                        }),
//                        contentType: 'application/json',
//                        success: function (result) {
//                            alert(result);
//                            // 修改成功
//                            window.location.href = '/OrganizerRoute/Index';
//                        },
//                        error: function (err) {
//                            alert(err.responseText);
//                        }
//                    });
//                },
//                error: function (textStatus, errorThrown) {
//                    $('#status').text('Error uploading image.');
//                    console.error('Error uploading image:', textStatus, errorThrown);
//                }
//            });
//        }
//    }
//});

// vue上傳圖片：https://hao1229.github.io/2019/08/05/EcommercePractice5/

var vueApp = {
    data() {
        return {
            account: '',
            name: '',
            URL: '',
            phone: '',
            email_old: '',
            emal_new: '',
            password_confirm: '',
            photoName: '',
        };
    },
    watch: {
        
    },
    methods: {
        // 顯示活動方基本資料
        showOrganizerInformation() {
            // 依照活動id取得活動資訊
            axios.get(`/api/OrganizersApi/info/${this.getOrganizerId()}`)
                .then(response => {
                    console.log(response.data);
                    const info = response.data;
                    this.account = info.organizerAccount;
                    this.name = info.organizerName;
                    this.URL = info.reservationUrl;
                    this.phone = info.phone;
                    this.email_old = info.email;
                    this.photoName = info.organizerPhoto;
                })
                .catch(error => {
                    console.error('取得活動方資訊時發生錯誤:', error);
                });
        },
        getOrganizerId() {
            // 透過Session取得活動方的id
            let organizerid = sessionStorage.getItem("OrganizerId");
            console.log(`OrganizerId：${organizerid}`);

            // 未登入時，跳到登入頁
            if (organizerid == null) {
                window.location.href = '/OrganizerRoute/OrganizerLogin';
            }
            return organizerid;
        },
        // 送出修改基本資料表單
        submitEditInfoForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 editActivity 方法
                this.editInfo();
            }
        },
        // 送出修改Email表單
        submitEditEmailForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 editActivity 方法
                this.editEmail();
            }
        },
        // 送出修改密碼表單
        submitEditPasswordForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 editActivity 方法
                this.editPassword();
            }
        },
        // 送出修改活動方照片表單
        submitEditPhotoForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 editActivity 方法
                this.editPassword();
            }
        },
        // 修改基本資料的操作
        editInfo() {
            let _this = this;
            var request = {};
            request.organizerName = _this.name;
            request.reservationUrl = _this.URL;
            request.phone = _this.phone;
        },
        // 修改Email的操作
        editEmail() {

        },
        // 修改密碼的操作
        editPassword() {

        },
        editPhoto() {

        },
        photopreview() {
            // 上傳圖片預覽
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
        }
    },
    // 在應用程式創建時立即執行方法
    created() {
        this.getOrganizerId(); 
        this.photopreview();
        this.showOrganizerInformation();
    },
};
var app = Vue.createApp(vueApp).mount("#app");
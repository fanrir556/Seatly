// vue上傳圖片：https://hao1229.github.io/2019/08/05/EcommercePractice5/

var vueApp = {
    data() {
        return {
            StartTime: null,
            EndTime: null,
            Capacity: null,
            ActivityName: null,
            DescriptionN: null,
            RecurringTime: null,
            IsRecurring: '',
            options: {
                True: '是',
                False: '否'
            }
        };
    },
    methods: {
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
        submitAddForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 addActivity 方法
                this.addActivity();
            }
        },
        // 新增活動
        addActivity() {
            // 透過Session取得活動方的id並轉為數字
            let organizeridInt = parseInt(`${sessionStorage.getItem("OrganizerId")}`);

            // 讀取圖片
            const uploadedFile = this.$refs.files.files[0]

            const reader = new FileReader()
            // 轉換成 Data URL
            reader.readAsDataURL(uploadedFile);

            reader.onload = function () {
                // 將 Data URL 轉成 Base64 字串
                const base64String = reader.result.split(',')[1]
                // 將 Base64 字串轉成 byte 陣列
                const byteArray = []
                for (let i = 0; i < base64String.length; i += 4) {
                    const bytes = atob(base64String.slice(i, i + 4))
                    for (let j = 0; j < bytes.length; j++) {
                        byteArray.push(bytes.charCodeAt(j))
                    }
                }
                // 輸出 varbinary 字串
                console.log(byteArray.join(','))
                const vbUploadedFile = new Blob([new Uint8Array(byteArray)]); // 轉換回 Blob 對象

                // 建立formdata
                const formData = new FormData()

                // 将需要上传的数据添加到 FormData 对象中
                formData.append('OrganizerId', organizeridInt);
                formData.append('ActivityPhoto', vbUploadedFile, uploadedFile.name); // 添加被轉換成 Blob 的圖片
                formData.append('StartTime', this.StartTime);
                formData.append('EndTime', this.EndTime);
                formData.append('Capacity', this.Capacity);
                formData.append('ActivityName', this.ActivityName);
                formData.append('DescriptionN', this.DescriptionN);
                formData.append('RecurringTime', this.RecurringTime);
                formData.append('IsRecurring', Boolean(this.IsRecurring));

                // 发送 POST 请求
                axios.post('/api/OrganizersApi/activity', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                    }
                })
                    .then(function (response) {
                        console.log(response);
                        alert("新增活動成功");
                    })
                    .catch(function (error) {
                        console.log(error);
                        alert("新增活動失敗");
                    });
            }
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
        },
    },
    // 在應用程式創建時立即執行方法
    created() {
        this.getOrganizerId();
        this.photopreview();
    },
};
var app = Vue.createApp(vueApp).mount("#app");

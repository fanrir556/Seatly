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
            ActivityMethod: '',
            RecurringOptions: {
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
        submitForm() {
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

            // 在回調函數外部保存 `this` 的值
            const self = this;

            reader.onload = function () {

                // reader.result 包含了讀取到的二進位資料
                var binaryData = reader.result;
                console.log('Binary Data:', binaryData);

                var blob = new Blob([binaryData]);
                console.log('blob Data:', blob);

                // 建立formdata
                const formData = new FormData()

                // 将需要上传的数据添加到 FormData 对象中
                formData.append('OrganizerId', organizeridInt);
                formData.append('ActivityPhoto', blob); // 添加被轉換成 Blob 的圖片
                formData.append('StartTime', self.StartTime);
                formData.append('EndTime', self.EndTime);
                formData.append('Capacity', self.Capacity);
                formData.append('ActivityName', self.ActivityName);
                formData.append('ActivityMethod', self.ActivityMethod);
                formData.append('DescriptionN', self.DescriptionN);
                formData.append('RecurringTime', self.RecurringTime);
                formData.append('IsRecurring', Boolean(self.IsRecurring));

                // 发送 POST 请求
                axios.post('/api/OrganizersApi/activity', formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                    }
                })
                    .then(function (response) {
                        console.log(response);
                        alert("新增活動成功");
                        window.location.href = './NotificationRecord';
                    })
                    .catch(function (error) {
                        console.log(error);
                        alert("新增活動失敗");
                    });
            }
            // 讀取檔案為 ArrayBuffer
            reader.readAsArrayBuffer(uploadedFile);
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
    },
};
var app = Vue.createApp(vueApp).mount("#app");

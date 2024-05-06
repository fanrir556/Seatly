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
        // 上傳照片
        uploadFile() {
            console.log(this)
            const uploadedFile = this.$refs.files.files[0]
            const vm = this
            const formData = new FormData()
            formData.append('ActivityPhoto', uploadedFile)
            const url = `/api/OrganizersApi/activity`
            this.$http.post(url, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            }).then((response) => {
                if (response.data.success) {
                    vm.$set(vm.tempProduct, 'imageUrl', response.data.imageUrl)
                }
            })
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
        addActivity() {
            // 透過Session取得活動方的id並轉為數字
            let organizeridInt = parseInt(`${sessionStorage.getItem("OrganizerId")}`); 

            // Send a POST request
            axios.post('/api/OrganizersApi/activity', {
                OrganizerId: organizeridInt,
                ActivityPhoto: null,
                StartTime: this.StartTime,
                EndTime: this.EndTime,
                Capacity: this.Capacity,
                ActivityName: this.ActivityName,
                DescriptionN: this.DescriptionN,
                RecurringTime: this.RecurringTime,
                IsRecurring: Boolean(this.IsRecurring), // 字串轉成布林值傳送
            })
                .then(function (response) {
                    console.log(response);
                })
                .catch(function (error) {
                    console.log(error);
                });
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

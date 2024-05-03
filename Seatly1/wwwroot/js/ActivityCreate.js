var vueApp = {
    components: { VueDatePicker },

    data() {
        return {
            form: {
                StartTime: null,
                // 其他表單數據...
            }
        };
    },
    methods: {
        getOrganizerId() {
            // 透過Session取得活動方的id
            const organizerid = sessionStorage.getItem("OrganizerId");
            console.log(`OrganizerId：${organizerid}`);

            // 未登入時，跳到登入頁
            if (organizerid == null) {
                window.location.href = '/OrganizerRoute/OrganizerLogin';
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
        addActivity() {
            console.log(this.form);  // 在這裡，您可以看到 form.StartTime 已經被更新為 datepicker 的值
        },
        Validation() {
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
        }
    },
    // 在應用程式創建時立即執行方法
    created() {
        this.getOrganizerId();
        this.photopreview();
        this.Validation();
    }
};
var app = Vue.createApp(vueApp).mount("#app");

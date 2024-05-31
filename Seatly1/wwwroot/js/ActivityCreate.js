// vue上傳圖片：https://hao1229.github.io/2019/08/05/EcommercePractice5/

var vueApp = {
    data() {
        return {
            StartTime: '',
            EndTime: '',
            Capacity: '',
            ActivityName: '',
            ActivityMethod: '',
            hashtag1: '',
            hashtag2: '',
            hashtag3: '',
            hashtag4: '',
            hashtag5: '',
            location: '',
            LocationDescrption: '',
        };
    },
    watch: {
        // 當StartTime或EndTime值改變時，執行以下程式，當開始時間比結束時間晚時，將結束時間設定成開始時間加1天
        StartTime(newStartTime) {
            if (newStartTime >= this.EndTime) {
                let starttime = new Date(newStartTime);
                let endtime = new Date(starttime.setDate(starttime.getDate() + 1));

                // 將日期時間轉換為v-model可以接受的格式
                let year = endtime.getFullYear();
                let month = ("0" + (endtime.getMonth() + 1)).slice(-2); // Months are zero based
                let day = ("0" + endtime.getDate()).slice(-2);
                let hour = ("0" + endtime.getHours()).slice(-2);
                let minute = ("0" + endtime.getMinutes()).slice(-2);
                let formattedDate = `${year}-${month}-${day}T${hour}:${minute}`;

                this.EndTime = formattedDate;
                console.log('結束時間' + this.EndTime);
                }
        },
        EndTime(newEndTime) {
            if (newEndTime <= this.StartTime) {
                let endtime = new Date(newEndTime);
                let starttime = new Date(endtime.setDate(endtime.getDate() - 1));

                // 將日期時間轉換為v-model可以接受的格式
                let year = starttime.getFullYear();
                let month = ("0" + (starttime.getMonth() + 1)).slice(-2); // Months are zero based
                let day = ("0" + starttime.getDate()).slice(-2);
                let hour = ("0" + starttime.getHours()).slice(-2);
                let minute = ("0" + starttime.getMinutes()).slice(-2);

                let formattedDate = `${year}-${month}-${day}T${hour}:${minute}`;
                this.StartTime = formattedDate;
                console.log('開始時間' + this.StartTime);
            }
        },
    },
    methods: {
        // demo用一鍵輸入資料
        demo() {
            this.StartTime = "2024-05-31T10:00";
            this.EndTime = "2024-05-31T15:30";
            this.Capacity= "30";
            this.ActivityName = "安平樹屋導覽解說";
            this.ActivityMethod = "排隊";
            this.hashtag1 = "戶外體驗";
            this.hashtag2 = "親子";
            this.hashtag3 = "其他生活娛樂";
            this.hashtag4 = "其他學習成長";
            this.hashtag5 = "公益";
            this.location = "台南";
            this.LocationDescrption = "708台南市安平區古堡街104號";
        },
        // 活動開始必須早於結束時間
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
                // 將讀取到的二進位資料轉換為blbo物件
                var blob = new Blob([reader.result]);

                // 建立formdata
                const formData = new FormData()
                console.log(self.ActivityMethod);

                if (self.ActivityMethod == '公告') {
                    // 活動方法為公告時，FormData不讀取活動人數上限的資料
                    formData.append('OrganizerId', organizeridInt);
                    formData.append('ActivityPhoto', blob); // 添加被轉換成 Blob 的圖片
                    formData.append('StartTime', self.StartTime);
                    formData.append('EndTime', self.EndTime);
                    formData.append('Capacity', 0);
                    formData.append('ActivityName', self.ActivityName);
                    formData.append('ActivityMethod', self.ActivityMethod);
                    formData.append('isActivity', true); // 預設啟用活動
                    formData.append('Location', self.location);
                    formData.append('LocationDescription', self.LocationDescrption);
                    formData.append('HashTag1', self.hashtag1);
                    formData.append('HashTag2', self.hashtag2);
                    formData.append('HashTag3', self.hashtag3);
                    formData.append('HashTag4', self.hashtag4);
                    formData.append('HashTag5', self.hashtag5);

                    // 发送 POST 请求
                    axios.post('/api/OrganizersApi/activity', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                        }
                    })
                        .then(function (response) {
                            console.log(response);
                            let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                            alert("新增活動成功，請進行活動描述的編輯");
                            window.location.href = `./Description/${newActivityId}`; // 将新活动的id添加到URL中
                        })
                        .catch(function (error) {
                            console.log(error);
                            alert("新增活動失敗");
                        });
                }
                else {
                    // 活動方法為公告時，FormData不讀取活動名稱跟活動人數上限的資料
                    formData.append('OrganizerId', organizeridInt);
                    formData.append('ActivityPhoto', blob); // 添加被轉換成 Blob 的圖片
                    formData.append('StartTime', self.StartTime);
                    formData.append('EndTime', self.EndTime);
                    formData.append('Capacity', self.Capacity);
                    formData.append('ActivityName', self.ActivityName);
                    formData.append('ActivityMethod', self.ActivityMethod);
                    formData.append('isActivity', true); // 預設啟用活動
                    formData.append('Location', self.location);
                    formData.append('LocationDescription', self.LocationDescrption);
                    formData.append('HashTag1', self.hashtag1);
                    formData.append('HashTag2', self.hashtag2);
                    formData.append('HashTag3', self.hashtag3);
                    formData.append('HashTag4', self.hashtag4);
                    formData.append('HashTag5', self.hashtag5);

                    // 发送 POST 请求
                    axios.post('/api/OrganizersApi/activity', formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                        }
                    })
                        .then(function (response) {
                            console.log(response);
                            let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                            alert("新增活動成功，請進行活動描述的編輯");
                            window.location.href = `./Description/${newActivityId}`; // 将新活动的id添加到URL中
                        })
                        .catch(function (error) {
                            console.log(error);
                            alert("新增活動失敗");
                        });
                }
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

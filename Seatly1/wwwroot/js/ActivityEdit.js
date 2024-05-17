// vue上傳圖片：https://hao1229.github.io/2019/08/05/EcommercePractice5/

var vueApp = {
    data() {
        return {
            StartTime: null,
            EndTime: null,
            Capacity: null,
            ActivityName: null,
            ActivityMethod: '',
            ActivityPhoto: null,
        };
    },
    methods: {
        getActivityInfo() {
            // 取得該活動資訊頁面網址最後的活動id
            const url = window.location.pathname;
            const activityId = url.substring(url.lastIndexOf('/') + 1);

            // 依照活動id取得活動資訊
            axios.get(`/api/OrganizersApi/activity/${activityId}`)
                .then(response => {
                    console.log(response.data);
                    const activity = response.data;
                    this.ActivityPhoto = this.binaryStringToBlob(activity.activityPhoto);
                    this.StartTime = this.convertToPM(activity.startTime);
                    this.EndTime = this.convertToPM(activity.endTime);
                    this.Capacity = activity.capacity
                    this.ActivityName = activity.activityName;
                    this.ActivityMethod = activity.activityMethod;

                    // blob 物件轉換成圖片
                    const fileReader = new FileReader();

                    fileReader.onload = e => {
                        this.ActivityPhoto = e.target.result; // 顯示活動圖片
                    };
                    fileReader.readAsDataURL(this.ActivityPhoto);
                })
                .catch(error => {
                    console.error('取得活動資訊時發生錯誤:', error);
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
        submitForm() {
            // 执行 Bootstrap 5 表单验证
            let forms = document.querySelectorAll('.needs-validation');
            Array.prototype.slice.call(forms).forEach(function (form) {
                form.classList.add('was-validated');
            });

            // 检查是否通过验证
            if (document.querySelectorAll('.was-validated :invalid').length === 0) {
                // 通过验证，调用 editActivity 方法
                this.editActivity();
            }
        },
        // 修改活動
        editActivity() {
            // 取得該活動資訊頁面網址最後的活動id
            const url = window.location.pathname;
            const activityId = url.substring(url.lastIndexOf('/') + 1);

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

                // 发送 put 请求
                axios.put(`/api/OrganizersApi/activity/${activityId}`, formData, {
                    headers: {
                        'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                    }
                })
                    .then(function (response) {
                        console.log(response);
                        alert("修改活動成功");
                        window.location.href = `/OrganizerRoute/Activity/${activityId}`;
                    })
                    .catch(function (error) {
                        console.log(error);
                        alert("修改活動失敗");
                    });
            }
            // 讀取檔案為 ArrayBuffer
            reader.readAsArrayBuffer(uploadedFile);
        },
        // 二進位字串轉換成 blob 物件
        binaryStringToBlob(binaryString, contentType) {
            contentType = contentType || '';
            const sliceSize = 512;
            const byteCharacters = atob(binaryString);
            const byteArrays = [];

            for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
                const slice = byteCharacters.slice(offset, offset + sliceSize);

                const byteNumbers = new Array(slice.length);
                for (let i = 0; i < slice.length; i++) {
                    byteNumbers[i] = slice.charCodeAt(i);
                }

                const byteArray = new Uint8Array(byteNumbers);
                byteArrays.push(byteArray);
            }

            const blob = new Blob(byteArrays, { type: contentType });
            return blob;
        },
        convertToPM(dateTimeString) {
            // 如果日期時間字串包含 'T'，則將 'T' 去除
            if (dateTimeString.includes('T')) {
                // 將 'T' 取代為空格
                return dateTimeString.replace('T', ' ');
            }
            // 如果不包含 'T'，則直接返回原始字串
            return dateTimeString;
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
        this.getActivityInfo();
    },
};
var app = Vue.createApp(vueApp).mount("#app");
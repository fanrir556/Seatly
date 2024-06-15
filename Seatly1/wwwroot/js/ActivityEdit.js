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
            ActivityPhoto: '',
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
                    this.hashtag1 = activity.hashTag1;
                    this.hashtag2 = activity.hashTag2;
                    this.hashtag3 = activity.hashTag3;
                    this.hashtag4 = activity.hashTag4;
                    this.hashtag5 = activity.hashTag5;
                    this.location = activity.location;
                    this.LocationDescrption = activity.locationDescription;

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

            // 有重新上傳封面圖片時才執行if內的程式，防止修改活動資訊沒有要變更圖片的時候需要重新上傳圖片
            if (uploadedFile != null) {
                const reader = new FileReader()

                // 在回調函數外部保存 `this` 的值
                const self = this;

                reader.onload = function () {
                    // 將讀取到的二進位資料轉換為blbo物件
                    var blob = new Blob([reader.result]);
                    console.log('blob Data:', blob);

                    // 建立formdata
                    const formData = new FormData()

                    if (self.ActivityMethod == '公告') {

                        // 活動方法為公告時，活動人數上限設為0
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
                        
                        // 為防止Hashtag輸入null字串，檢查每個HashTag，如果值不為 null，則添加到 FormData
                        if (self.hashtag1 !== null) {
                            formData.append('HashTag1', self.hashtag1);
                        }
                        if (self.hashtag2 !== null) {
                            formData.append('HashTag2', self.hashtag2);
                        }
                        if (self.hashtag3 !== null) {
                            formData.append('HashTag3', self.hashtag3);
                        }
                        if (self.hashtag4 !== null) {
                            formData.append('HashTag4', self.hashtag4);
                        }
                        if (self.hashtag5 !== null) {
                            formData.append('HashTag5', self.hashtag5);
                        }

                        // 发送 patch 请求
                        axios.patch(`/api/OrganizersApi/activity/${activityId}`, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                            }
                        })
                            .then(function (response) {
                                console.log(response);
                                let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                                alert("修改活動成功，請進行活動描述的編輯");
                                window.location.href = `../Description/${newActivityId}`; // 将新活动的id添加到URL中
                            })
                            .catch(function (error) {
                                console.log(error);
                                alert("修改活動失敗");
                            });
                    }
                    else {
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

                        // 為防止Hashtag輸入null字串，檢查每個HashTag，如果值不為 null，則添加到 FormData
                        if (self.hashtag1 !== null) {
                            formData.append('HashTag1', self.hashtag1);
                        }
                        if (self.hashtag2 !== null) {
                            formData.append('HashTag2', self.hashtag2);
                        }
                        if (self.hashtag3 !== null) {
                            formData.append('HashTag3', self.hashtag3);
                        }
                        if (self.hashtag4 !== null) {
                            formData.append('HashTag4', self.hashtag4);
                        }
                        if (self.hashtag5 !== null) {
                            formData.append('HashTag5', self.hashtag5);
                        }

                        // 发送 patch 请求
                        axios.patch(`/api/OrganizersApi/activity/${activityId}`, formData, {
                            headers: {
                                'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                            }
                        })
                            .then(function (response) {
                                console.log(response);
                                let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                                alert("修改活動成功，請進行活動描述的編輯");
                                window.location.href = `../Description/${newActivityId}`; // 将新活动的id添加到URL中
                            })
                            .catch(function (error) {
                                console.log(error);
                                alert("修改活動失敗");
                            });
                    }
                }
                // 讀取檔案為 ArrayBuffer
                reader.readAsArrayBuffer(uploadedFile);
            }
            else {
                // 在回調函數外部保存 `this` 的值
                const self = this;

                // 建立formdata
                const formData = new FormData()

                if (self.ActivityMethod == '公告') {
                    // 活動方法為公告時，活動人數上限設為0
                    formData.append('OrganizerId', organizeridInt);
                    formData.append('StartTime', self.StartTime);
                    formData.append('EndTime', self.EndTime);
                    formData.append('Capacity', 0);
                    formData.append('ActivityName', self.ActivityName);
                    formData.append('ActivityMethod', self.ActivityMethod);
                    formData.append('isActivity', true); // 預設啟用活動
                    formData.append('Location', self.location);
                    formData.append('LocationDescription', self.LocationDescrption);

                    // 為防止Hashtag輸入null字串，檢查每個HashTag，如果值不為 null，則添加到 FormData
                    if (self.hashtag1 !== null) {
                        formData.append('HashTag1', self.hashtag1);
                    }
                    if (self.hashtag2 !== null) {
                        formData.append('HashTag2', self.hashtag2);
                    }
                    if (self.hashtag3 !== null) {
                        formData.append('HashTag3', self.hashtag3);
                    }
                    if (self.hashtag4 !== null) {
                        formData.append('HashTag4', self.hashtag4);
                    }
                    if (self.hashtag5 !== null) {
                        formData.append('HashTag5', self.hashtag5);
                    }

                    // 发送 patch 请求
                    axios.patch(`/api/OrganizersApi/activity/${activityId}`, formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                        }
                    })
                        .then(function (response) {
                            console.log(response);
                            let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                            alert("修改活動成功，請進行活動描述的編輯");
                            window.location.href = `../Description/${newActivityId}`; // 将新活动的id添加到URL中
                        })
                        .catch(function (error) {
                            console.log(error);
                            alert("修改活動失敗");
                        });
                }
                else {
                    formData.append('OrganizerId', organizeridInt);
                    formData.append('StartTime', self.StartTime);
                    formData.append('EndTime', self.EndTime);
                    formData.append('Capacity', self.Capacity);
                    formData.append('ActivityName', self.ActivityName);
                    formData.append('ActivityMethod', self.ActivityMethod);
                    formData.append('isActivity', true); // 預設啟用活動
                    formData.append('Location', self.location);
                    formData.append('LocationDescription', self.LocationDescrption);

                    // 為防止Hashtag輸入null字串，檢查每個HashTag，如果值不為 null，則添加到 FormData
                    if (self.hashtag1 !== null) {
                        formData.append('HashTag1', self.hashtag1);
                    }
                    if (self.hashtag2 !== null) {
                        formData.append('HashTag2', self.hashtag2);
                    }
                    if (self.hashtag3 !== null) {
                        formData.append('HashTag3', self.hashtag3);
                    }
                    if (self.hashtag4 !== null) {
                        formData.append('HashTag4', self.hashtag4);
                    }
                    if (self.hashtag5 !== null) {
                        formData.append('HashTag5', self.hashtag5);
                    }

                    // 发送 patch 请求
                    axios.patch(`/api/OrganizersApi/activity/${activityId}`, formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data' // 设置请求头为 multipart/form-data
                        }
                    })
                        .then(function (response) {
                            console.log(response);
                            let newActivityId = response.data; // 假设服务器返回的响应中包含新活动的id
                            alert("修改活動成功，請進行活動描述的編輯");
                            window.location.href = `../Description/${newActivityId}`; // 将新活动的id添加到URL中
                        })
                        .catch(function (error) {
                            console.log(error);
                            alert("修改活動失敗");
                        });
                }
            }
            
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
var vueApp = {
    data() {
        return {
            ActivityId: null,
            ActivityPhoto: null,
            StartTime: null,
            EndTime: null,
            Capacity: null,
            ActivityName: null,
            ActivityMethod: null,
            DescriptionN: null,
            HashTag1: null,
            HashTag2: null,
            HashTag3: null,
            HashTag4: null,
            HashTag5: null,
            openDeleteModal: false,
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
        getActivityId() {
            // 取得該活動資訊頁面網址最後的活動id
            const url = window.location.pathname;
            const activityId = url.substring(url.lastIndexOf('/') + 1);
            return activityId;
        },
        getActivityInfo() {
            // 依照活動id取得活動資訊
            axios.get(`/api/OrganizersApi/activity/${this.getActivityId()}`)
                .then(response => {
                    console.log(response.data);
                    const activity = response.data;
                    this.ActivityId = activity.activityId;
                    this.ActivityPhoto = this.binaryStringToBlob(activity.activityPhoto);
                    this.StartTime = this.convertToPM(activity.startTime);
                    this.EndTime = this.convertToPM(activity.endTime);
                    this.Capacity = activity.capacity;
                    this.ActivityName = activity.activityName;
                    this.ActivityMethod = activity.activityMethod;
                    this.DescriptionN = activity.descriptionN;
                    this.HashTag1 = activity.hashTag1;
                    this.HashTag2 = activity.hashTag2;
                    this.HashTag3 = activity.hashTag3;
                    this.HashTag4 = activity.hashTag4;
                    this.HashTag5 = activity.hashTag5;
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
        convertToPM(dateTimeString) {
            // 如果日期時間字串包含 'T'，則將 'T' 去除
            if (dateTimeString.includes('T')) {
                // 將 'T' 取代為空格
                return dateTimeString.replace('T', ' ');
            }
            // 如果不包含 'T'，則直接返回原始字串
            return dateTimeString;
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
        edit() {
            window.location.href = `/OrganizerRoute/ActivityEdit/${this.getActivityId()}`;
        },
        deleteActivity() {
            axios.delete(`/api/OrganizersApi/activity/${this.getActivityId()}`)
                .then(response => {
                    console.log(response.data);
                    window.location.href = `/OrganizerRoute/NotificationRecord`;
                }).catch(err => {
                    console.log(err);
                });
        },
    },
    // 在應用程式創建時立即執行方法
    created() {
        this.getOrganizerId();
        this.getActivityInfo();
    },
};
var app = Vue.createApp(vueApp).mount("#app");

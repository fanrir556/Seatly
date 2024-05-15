
    //路徑
    window.vuebase = "https://localhost:7271";

    //活動方id && 活動專屬ID
    window.organizerid = sessionStorage.getItem("OrganizerId");
    // console.log(organizerid)
    window.ActId = sessionStorage.getItem("ActID");
    // console.log(ActId)


    //掛載Vue
    const vueApp = {
        data() {
            return {
        //活動資訊
        activityId: "",
    activityName: "",
    startTime: "",
    endTime: "",

    //排隊資訊
    BookOrder: [],
    orderId: "",
    userName: "",
    waitingNumber: "",
    activityBarcode: "",
    checked: "",
            }
        },
    methods: {
        //活動資訊
        ActInfo: function () {
        // alert("list"); // 可以暫時移除
        let _this = this;

    axios.get(`${vuebase}/Confirm/ActiveInfo/${ActId}`)
                    .then(response => {
                        // 在這裡處理成功的回應
                        var res = response.data
    // console.log(res);
    _this.activityId = res[0].activityId

    if (res[0].activityName != null) {
        _this.activityName = res[0].activityName
    }
    else {
        _this.activityName = "沒有名稱"
    }

    _this.startTime = moment(res[0].StartTime).format('YYYY-MM-DD/HH')
    _this.endTime = moment(res[0].EndTime).format('YYYY-MM-DD/HH')
                    })
                    .catch(err => {
        alert(err);
                        // 在這裡處理失敗的回應
                    });
            },
    //排隊訂單資訊
    BookInfo: function () {
        let _this = this;
    // alert("Book")
    axios.get(`${vuebase}/Confirm/BookInfo/${ActId}`)
                    .then(response => {
        // console.log(response)
        res = response.data
                        // 清空 BookOrder 陣列
                        _this.BookOrder = [];

                        // 將每個項目添加到 BookOrder 陣列中
                        res.forEach(item => {
        _this.BookOrder.push({
            orderId: item.orderId,
            userName: item.userName,
            waitingNumber: item.waitingNumber,
            activityBarcode: item.activityBarcode,
            checked: item.checked
        });
                        });
                    })
                    .catch(err => {
        alert(err)
    });
            },
    //簽到按鈕
    check: function () {
        let _this = this;
    // alert("check");
    // 從事件對象(event object)中取得觸發事件的按鈕元素
    const btn = event.target;

    // 從按鈕元素中取得自定義屬性"btn-id"
    const btnBarcode = btn.getAttribute("btn-Barcode");
    // console.log(btnBarcode);
    const waitNum1 = btn.getAttribute("waitNum");
    // console.log(waitNum1);
    var ret = confirm("確認簽到?");
    if (ret == true) {
        axios.post(`${vuebase}/Confirm/TransCheck/?Barcode=${btnBarcode}&waitNum=${waitNum1}`)
            .then(res => { _this.BookInfo(); })
            .catch(err => { alert(err) })
    }
            },
    uncheck: function () {
        let _this = this;
    // alert("check");
    // 從事件對象(event object)中取得觸發事件的按鈕元素
    const btn = event.target;

    // 從按鈕元素中取得自定義屬性"btn-id"
    const btnBarcode = btn.getAttribute("btn-Barcode");
    // console.log(btnBarcode);
    const waitNum1 = btn.getAttribute("waitNum");
    // console.log(waitNum1);


    var ret = confirm("確認解除?");
    if (ret == true) {
        axios.post(`${vuebase}/Confirm/TransUnCheck/?Barcode=${btnBarcode}&waitNum=${waitNum1}`)
            .then(res => { _this.BookInfo(); })
            .catch(err => { alert(err) })
    }
            
            }
        },
    mounted: function () {
        let _this = this;
    _this.ActInfo();
    _this.BookInfo();
        }
    };

    Vue.createApp(vueApp).mount("#app");

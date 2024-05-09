// 透過Session取得活動方的id
const organizerid = sessionStorage.getItem("OrganizerId");
console.log(`OrganizerId：${organizerid}`);

// 未登入時，跳到登入頁
if (organizerid == null) {
    window.location.href = '/OrganizerRoute/OrganizerLogin';
}

// 要顯示的資料筆數
count = 50;

// 顯示活動資訊
$.ajax({
    url: `/api/OrganizersApi/activities/${organizerid}/${count}`, // 后端 API 的 URL
    type: 'GET',
    success: function (activities) {
        console.log(activities);
        activities.forEach(function (activity) {
            read(activity);
        });
    },
    error: function (err) {
        console.log(err.responseText)
    }
});

function convertToPM(dateTimeString) {
    // 如果日期時間字串包含 'T'，則將 'T' 去除
    if (dateTimeString.includes('T')) {
        // 將 'T' 取代為空格加上 'PM' 標記
        return dateTimeString.replace('T', ' ');
    }
    // 如果不包含 'T'，則直接返回原始字串
    return dateTimeString;
}

// Function to add a new row
function read(activity) {
    activity.startTime = convertToPM(activity.startTime);
    activity.endTime = convertToPM(activity.endTime);
    activityPhoto = binaryStringToBlob(activity.activityPhoto);
    console.log(activityPhoto);

    // blob物件轉換成圖片
    const fileReader = new FileReader();

    fileReader.onload = e => {
        $("#activities").append(`
            <div id="card" class="col-12 col-md-4 col-lg-3 mb-5">
                <div class="card">
                    <img src="${e.target.result}" style="height: 200px;  width: 50%;" class="d-block w-100 card-img-top" alt="https://placehold.co/300x250">
                    <div class="card-body">
                        <h5 class="card-title">${activity.activityName}</h5>
                    </div>
                </div>
            </div>
        `)
    };
    fileReader.readAsDataURL(activityPhoto);
}

// 二進位字串轉換成blob物件
function binaryStringToBlob(binaryString, contentType) {
    contentType = contentType || '';
    var sliceSize = 512;
    var byteCharacters = atob(binaryString);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}

// 新增活動
function add() {
    window.location.href = '/OrganizerRoute/ActivityCreate';
}
// 修改活動
function edit() {
    window.location.href = '/OrganizerRoute/ActivityEdit';
}
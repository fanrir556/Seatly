// 透過Session取得活動方的id
const organizerid = sessionStorage.getItem("OrganizerId");
console.log(`OrganizerId：${organizerid}`);

// 未登入時，跳到登入頁
if (organizerid == null) {
    window.location.href = '/OrganizerRoute/OrganizerLogin';
}

// 顯示活動資訊
$.ajax({
    url: `/api/OrganizersApi/activities/${organizerid}`, // 后端 API 的 URL
    type: 'GET',
    success: function (activities) {
        console.log(activities);
        activities.forEach(function (activity) {
            readRow(activity);
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
function readRow(activity) {
    activity.startTime = convertToPM(activity.startTime);
    activity.endTime = convertToPM(activity.endTime);
    activityPhoto = binaryStringToBlob(activity.activityPhoto);
    console.log(activityPhoto);

    // blob物件轉換成圖片
    const fileReader = new FileReader();

    fileReader.onload = e => {
    var table = document.getElementById("dataTable").getElementsByTagName('tbody')[0];
    var newRow = table.insertRow();
        newRow.innerHTML = `
        <td>${activity.activityId}</td>
        <td><img style="width:240px;height:180px" src="${e.target.result}" alt="Activity Photo"></td>
        <td>${activity.startTime}</td>
        <td>${activity.endTime}</td>
        <td>${activity.capacity}</td>
        <td>${activity.activityName}</td>
        <td>${activity.activityMethod}</td>
        <td>${activity.descriptionN}</td>
        <td>${activity.isRecurring}</td>
        <td>${activity.recurringTime}</td>
        <td>
        <button type="button" class="btn btn-success btn-sm me-2" onclick="editRow(this)">修改</button>
        <button type="button" class="btn btn-danger btn-sm me-2" onclick="deleteRow(this)">刪除</button>
        </td>`;
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
function addRow() {
    window.location.href = '/OrganizerRoute/ActivityCreate';
}
// 修改活動
function editRow() {
    alert("edit");
}

// 刪除活動
function deleteRow() {
    alert("delete");
}
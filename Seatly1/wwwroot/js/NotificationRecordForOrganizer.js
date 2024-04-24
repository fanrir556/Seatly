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

// 示例使用
var dateTimeString = '2022-04-25T13:30:00';
var convertedDateTime = convertToPM(dateTimeString);
console.log(convertedDateTime); // 輸出：2022-04-25 13:30:00 下午

// Function to add a new row
function readRow(activity) {
    activity.startTime = convertToPM(activity.startTime);
    activity.endTime = convertToPM(activity.endTime);
    var table = document.getElementById("dataTable").getElementsByTagName('tbody')[0];
    var newRow = table.insertRow();
    newRow.innerHTML = `<td>${activity.activityId}</td>` +
        `<td>${activity.organizerId}</td>` +
        `<td>${null}</td>` +
        `<td>${activity.startTime}</td>` +
        `<td>${activity.endTime}</td>` +
        `<td>${activity.capacity}</td>` +
        `<td>${activity.activityName}</td>` +
        `<td>${activity.descriptionN}</td>` +
        `<td>${activity.isRecurring}</td>` +
        `<td>${activity.recurringTime}</td>` +
        '<td>' +
        '<button type="button" class="btn btn-success btn-sm me-2" onclick="editRow(this)">修改</button>' +
        '<button type="button" class="btn btn-danger btn-sm me-2" onclick="deleteRow(this)">刪除</button>' +
        '</td>';
}

function addRow(activity) {
    
}
// Function to edit a row
//function editRow(button) {
//    var row = button.parentNode.parentNode;
//    var cells = row.getElementsByTagName("td");
//    for (var i = 0; i < cells.length - 1; i++) {
//        cells[i].setAttribute("contenteditable", "true");
//    }
//}

//// Function to delete a row
//function deleteRow(button) {
//    var row = button.parentNode.parentNode;
//    row.parentNode.removeChild(row);
//}
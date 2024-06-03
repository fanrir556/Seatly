let editor;

ClassicEditor
    .create(document.querySelector('#editor'))
    .then(newEditor => {
        editor = newEditor;
        getDescription();
    })
    .catch(error => {
        console.error(error);
    });
function getOrganizerId() {
    // 透過Session取得活動方的id
    let organizerid = sessionStorage.getItem("OrganizerId");
    console.log(`OrganizerId：${organizerid}`);

    // 未登入時，跳到登入頁
    if (organizerid == null) {
        window.location.href = '/OrganizerRoute/OrganizerLogin';
    }
    return organizerid;
}

// 顯示編輯前資料
function getDescription() {
    // 取得該活動資訊頁面網址最後的活動id
    const url = window.location.pathname;
    const activityId = url.substring(url.lastIndexOf('/') + 1);

    // 透過 axios 發送 GET 請求根據活動id取得描述
    axios.get(`/api/OrganizersApi/activity/description/${activityId}`)  // 替換為您的 API 端點
        .then(response => {
            editor.setData(response.data.descriptionN);  // 顯示編輯前資料到編輯器的輸入框
        })
        .catch(error => {
            console.error(error);
        });
}

function photopreview() {
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
// 上傳描述圖片
function uploadpicture() {
    event.preventDefault(); // 阻止表单默认提交行为

    let formData = new FormData();
    let fileInput = document.getElementById('imageInput');
    let file = fileInput.files[0];

    // 获取当前日期和时间
    var currentDate = new Date();
    var dateString = currentDate.getFullYear() + ('0' + (currentDate.getMonth() + 1)).slice(-2) + ('0' + currentDate.getDate()).slice(-2);
    var timeString = ('0' + currentDate.getHours()).slice(-2) + ('0' + currentDate.getMinutes()).slice(-2) + ('0' + currentDate.getSeconds()).slice(-2);
    var fileExtension = file.name.split('.').pop(); // 获取文件的扩展名
    var fileName = dateString + '_' + timeString + '.' + fileExtension; // 文件名只保留附檔名，其他部分用日期和时间替换

    formData.append('image', file, fileName);

    // 使用axios发送请求到后端上传图片
    axios({
        url: '/api/OrganizersApi/uploads/description', // 后端上传图片的 URL
        method: 'POST',
        data: formData,
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    }).then(function (response) {
        console.log('Image uploaded successfully:', response);
        alert("上傳成功");

        // 使用 getData 方法來獲取當前的內容，然後再添加新的內容
        var currentData = editor.getData();
        editor.setData(currentData + `<img alt="描述圖片內容" src="../../../uploads/description/${fileName}">`); // 添加圖檔

    }).then(function (result) {
        console.log(result);
    }).catch(function (error) {
        if (error.response) {
            alert(error.response.data);
        } else if (error.request) {
            $('#status').text('Error uploading image.');
            console.error('Error uploading image:', error.request);
        } else {
            console.error('Error', error.message);
        }
    });
}
// 清除上傳預覽圖片
function clearImage() {
    $('#imageInput').val('');
    $('#preview').attr('src', '');
}
function submitForm() {
    if (editor) { // 檢查 'editor' 實例是否存在

        const editorData = editor.getData();
        console.log(editorData);

        // 取得該活動資訊頁面網址最後的活動id
        const url = window.location.pathname;
        const activityId = url.substring(url.lastIndexOf('/') + 1);

        axios({
            method: 'patch',
            url: `/api/OrganizersApi/activity/${activityId}`,
            data: {
                DescriptionN: editorData,
            }
        }).then(function (response) {
            console.log(response);
            alert("新增描述成功");
            window.location.href = `../NotificationRecord`; // 将新活动的id添加到URL中
        })
        .catch(function (error) {
            console.log(error);
            alert("新增描述失敗");
        });

    } else {
        console.error('未找到 CKEditor 實例');
    }
}

// 執行函式
getOrganizerId();
photopreview();
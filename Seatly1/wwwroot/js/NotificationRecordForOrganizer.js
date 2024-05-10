// 透過 Session 取得活動方的 ID
const organizerId = sessionStorage.getItem("OrganizerId");
console.log(`OrganizerId：${organizerId}`);

// 如果未登入，則導向登入頁面
if (organizerId == null) {
    window.location.href = '/OrganizerRoute/OrganizerLogin';
}

// 設定後端要傳送的資料筆數，可設定一頁要顯示幾筆
const count = 50;

// 顯示活動資訊
axios.get(`/api/OrganizersApi/activities/${organizerId}/${count}`)
    .then(response => {
        const activities = response.data;
        activities.forEach(activity => {
            read(activity);
        });
    })
    .catch(error => {
        console.error('取得活動資料時發生錯誤:', error);
    });

function convertToPM(dateTimeString) {
    // 如果日期時間字串包含 'T'，則將 'T' 去除
    if (dateTimeString.includes('T')) {
        // 將 'T' 取代為空格
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

    // blob 物件轉換成圖片
    const fileReader = new FileReader();

    fileReader.onload = e => {
        // 點擊圖片時進入活動資訊頁面，傳遞活動的 ID
        const card = document.createElement('div');
        card.className = 'col-12 col-md-4 col-lg-3 mb-5';
        card.innerHTML = `
            <div class="card">
                <img src="${e.target.result}" style="height: 200px;  width: 150px;" class="d-block w-100 card-img-top" alt="https://placehold.co/300x250" onclick="viewActivity(${activity.id})">
                <div class="card-body">
                    <p class="card-text">${activity.startTime} - ${activity.endTime}</p>
                    <h5 class="card-title">${activity.activityName}</h5>
                </div>
                <ul class="list-group list-group-flush">
                    <li class="list-group-item">#${activity.hashTag1} #${activity.hashTag2} #${activity.hashTag3} #${activity.hashTag4} #${activity.hashTag5}</li>
                </ul>
            </div>
        `;
        document.getElementById('activities').appendChild(card);
    };
    fileReader.readAsDataURL(activityPhoto);
}

// 二進位字串轉換成 blob 物件
function binaryStringToBlob(binaryString, contentType) {
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
}

// 進入活動資訊頁面
function viewActivity(activityId) {
    window.location.href = `/OrganizerRoute/Activity?id=${activityId}`;
}

// 新增活動
function add() {
    window.location.href = '/OrganizerRoute/ActivityCreate';
}

// 修改活動
function edit() {
    window.location.href = '/OrganizerRoute/ActivityEdit';
}
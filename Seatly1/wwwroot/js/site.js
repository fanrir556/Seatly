var strPoints = sessionStorage.getItem('strPoints') || "pointsShop";
var strCate = sessionStorage.getItem('strCate') || "all";
var isSearching = sessionStorage.getItem('isSearching');
var isTranSearching = sessionStorage.getItem("isTranSearching");
var isMG = null;
var isUser = sessionStorage.getItem("userLogin");
var mouseX = 0;
var mouseY = 0;

/*載入動畫*/
window.addEventListener('mousemove',(e) => {
    mouseX = e.clientX;
    mouseY = e.clientY;
});
const insertLoading = () => {
    var cursor = { x: mouseX, y: mouseY };

    document.querySelector('body').insertAdjacentHTML(
        'beforeend',
        `<div id="saveLoading" class="position-fixed" style="top: ${cursor.y}px; left: ${cursor.x}px;"><div role="img" class="wheel-and-queuely"><div class="wheel"></div><img src="${window.location.origin}/images/queuelyhome.png" class="queuely" /><div class="spoke"></div></div></div>`
    );
};

const followingLoading = (e) => {
    var cursor = { x: e.clientX, y: e.clientY };
    const saveLoading = document.querySelector('#saveLoading');
    if (saveLoading){
        saveLoading.style.top = `${cursor.y}px`;
        saveLoading.style.left = `${cursor.x}px`;
    }
};

const removeMouseMoveListener = () => {
    window.removeEventListener('mousemove', followingLoading);
};

const saveLoading = () => {
    insertLoading();
    window.addEventListener('mousemove', followingLoading);
};

const removeSaveLoading = () => {
    removeMouseMoveListener();
    const saveLoading = document.querySelector('#saveLoading');
    if (saveLoading){
        saveLoading.remove();
    }
};
/*載入動畫*/

/*管理員登入檢查*/
const MGCheck = () => {
    const checkIsMgActionUri = `${window.location.origin}/Home/CheckIsMG`;
    fetch(checkIsMgActionUri, { method: "GET" })
    .then(response => response.text())
    .then(data => {
        if (data !== "false") {
            sessionStorage.setItem("isManager","true");
        }
    })
    .catch(err => console.error('Error fetching MG status:', err.message));

    const ismg = sessionStorage.getItem("isManager");
    return ismg;
};

isMG = MGCheck();
/*管理員登入檢查*/

$(function () {
    removeSaveLoading();
    /*點數專區導覽列hover開始*/
    var timer;
    $("#pointsNav, #pointsDropdown").on("mouseenter", function () {
        clearTimeout(timer);
        $("#pointsNav").addClass("show");
        $("#pointsNav").attr("aria-expanded", "true");
        $("#pointsDropdown").addClass("show");
        $("#pointsDropdown").attr("data-bs-popper", "none");
    });

    $("#pointsNav, #pointsDropdown").on("mouseleave", function () {
        timer = setTimeout(function () {
            $("#pointsNav").removeClass("show");
            $("#pointsNav").attr("aria-expanded", "false");
            $("#pointsDropdown").removeClass("show");
            $("#pointsDropdown").attr("data-bs-popper", "");
        }, 100); // 這裡可以調整延遲的時間
    });
    /*點數專區導覽列hover結束*/

    /*點數專區導向partial*/
    $("#pointsShop").on("click", function () {
        strPoints = "pointsShop";
        sessionStorage.setItem('strPoints', strPoints);
    });
    $("#pointsHistory").on("click", function () {
        strPoints = "pointsHistory";
        sessionStorage.setItem('strPoints', strPoints);
    });
    $("#coupon").on("click", function () {
        strPoints = "coupon";
        sessionStorage.setItem('strPoints', strPoints);
    });
    /*點數專區導向partial*/

    /*會員登入檢測&隱藏活動方*/
    var isLogin = $("#isLogin").find("span");
    if (isLogin.length != 0) {
        //alert("tr");
        $("#OrganizersLoginRegisterNav").hide();
        $("#userCollections").show();
        sessionStorage.setItem("userLogin", "true");
    }
    else {
        //alert("fa");
        $("#OrganizersLoginRegisterNav").show();
        $("#userCollections").hide();
        sessionStorage.removeItem("userLogin");
    }
    /*會員登入隱藏活動方*/

    // Managermodal
    //var managerUrl = $("#mainModal").data("url");
    //$.ajax({
    //    type: "GET",
    //    url: managerUrl
    //}).done(function (data) {
    //    $("#mainModalBody").html(data);
    //}).fail(function (err) {
    //    alert(err.responseText);
    //});
    // Managermodal

    /* Logo小遊戲 */
    $("#home").on("mousedown", function (e) {
        setTimeout(function () {
            $("#logo").css("animation-name", "logoSpin")
        }, 1000);
        timer = setTimeout(function () {
            $("#logo").css("animation-name", "");
            var toast = new bootstrap.Toast($("#liveToast"))
            if ($("#logoed").index() != -1)
            {
                var logoGameUri = $("#logoed").data("uri");
                fetch(logoGameUri, {
                method: "GET",
            }).then(response => {
                return response.json();
            }).then(data => {
                //console.log(data);
                if (data == "今日已完成小遊戲") {
                    $("#liveToast>.toast-header").html(`<strong class="me-auto">今日完成次數達上限!</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                    $("#liveToast>.toast-body").text(`今日完成隱藏小遊戲次數已達上限`);
                }
                else {
                    $("#liveToast>.toast-header").html(`<strong class="me-auto">恭喜找到隱藏小遊戲</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                    $("#liveToast>.toast-body").text(`恭喜獲得${data[0]}點\n現在點數:${data[1]}\n今日已完成隱藏小遊戲次數:(${data[2]}/1)`);
                    var homeUri = `${$("#home").data("uri")}`
                        fetch(homeUri, {
                            method: "GET",
                        }).then(response => {
                            sessionStorage.setItem('userPointsEdited', 'true');
                        }).catch(err => {
                            alert(err);
                        });
                }

                toast.show();
            }).catch(err => {
                alert(err);
            });
            }
            else
            {
                $("#liveToast>.toast-header").html(`<strong class="me-auto">什麼都沒有發生</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                $("#liveToast>.toast-body").text(`恭喜找到小彩蛋`);
                toast.show();
            }
        }, 3000);
    });
    $("#home").on("mouseup", function () {
        $("#logo").css("animation-name", "")
        clearTimeout(timer);
    });
    /* Logo小遊戲 */

    /*每日簽到*/
    $("#checkIn").on("click", function () {
        var checkInUri = `${$(this).data("uri")}`;
        fetch(checkInUri, {
            method: "GET",
        }).then(response => {
            return response.json();
        }).then(data => {
            //console.log(data);
            var toast = new bootstrap.Toast($("#liveToast"))
            if (data == "今日已簽到") {
                $("#liveToast>.toast-header").html(`<strong class="me-auto">今日已簽到!</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                $("#liveToast>.toast-body").text(`今日已簽到完成`);
            }
            else {
                $("#liveToast>.toast-header").html(`<strong class="me-auto">每日簽到完成!</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                $("#liveToast>.toast-body").text(`恭喜獲得${data[0]}點\n現在點數:${data[1]}`);
                var homeUri = `${$("#home").data("uri")}`;
                fetch(homeUri, {
                    method: "GET",
                }).then(response => {
                    sessionStorage.setItem('userPointsEdited', 'true');
                }).catch(err => {
                    alert(err);
                });
            }

            toast.show();
            $(this).hide();
        }).catch(err => {
            alert(err);
        });
    });
    /*每日簽到*/

    /*小遊戲*/
    var gameSrc = $("#game").data("src");
    var gameUri = $("#game").data("uri");
    function gameLogoPop() {
        var gameTargets = $("main").find("div, p, h1, h2, h3, h4, h5, h6, li").not(".modal, .modal div, p, h1, h2, h3, h4, h5, h6, li");
        var randEl = Math.floor(Math.random() * gameTargets.length);
        var aniNum = Math.ceil(Math.random() * 4);
        var el = gameTargets.eq(randEl);
        var rect = el[0].getBoundingClientRect();
        var top = rect.top + window.scrollY;
        var left = rect.left + window.scrollX;
        var bottom = top + el.outerHeight();
        var right = left + el.outerWidth();
        var gamePosition = {x: 0, y: 0};
        if (aniNum == 1 || aniNum == 2) {
            gamePosition.y = top;
        }
        else{
            gamePosition.y = bottom;
        }
        if (aniNum == 1 || aniNum == 4) {
            gamePosition.x = left;
        }
        else{
            gamePosition.x = right;
        }
        $("body").append(`<div id="gameLogoDiv" class="position-fixed container-fluid p-0" style="width: 0px; height: 0px; top:${gamePosition.y}px; left:${gamePosition.x}px;"><div class="container-fluid position-relative p-0"><img id="gameLogo" class="position-absolute p-0" style="width: 30px; height: 30px; z-index: -1;" src=${gameSrc} /></div></div>`);
        $("#gameLogo").css("animation-name", `gameLogoShow${aniNum}`);
        $("#gameLogo").on("click", function () {
            //alert("恭喜找到小Q!");
            fetch(gameUri, {
                method: "GET",
            }).then(response => {
                return response.json();
            }).then(data => {
                //console.log(data);
                var toast = new bootstrap.Toast($("#liveToast"))
                if (data == "今日已完成小遊戲") {
                    $("#liveToast>.toast-header").html(`<strong class="me-auto">今日完成次數達上限!</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                    $("#liveToast>.toast-body").text(`今日完成小遊戲次數已達上限`);
                }
                else {
                    $("#liveToast>.toast-header").html(`<strong class="me-auto">恭喜找到小Q!</strong><button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Close"></button>`)
                    $("#liveToast>.toast-body").text(`恭喜獲得${data[0]}點\n現在點數:${data[1]}\n今日已完成小遊戲次數:(${data[2]}/3)`);
                    if (data[2] == 3) {
                        var homeUri = `${$("#home").data("uri")}`
                        fetch(homeUri, {
                            method: "GET",
                        }).then(response => {
                            sessionStorage.setItem('userPointsEdited', 'true');
                        }).catch(err => {
                            alert(err);
                        });
                    }
                }

                toast.show();
                $("#gameLogoDiv").remove();
                clearInterval(gameLoop);
            }).catch(err => {
                alert(err);
            });
        });
        setTimeout(function () {
            $("#gameLogoDiv").remove();
        }, 4000);
    }

    if ($("#game").index() != -1) {
        var gameLoop = setInterval(function () {
            gameLogoPop();
        }, 4100);
    }
    else {
        // 當你不再需要迴圈執行功能時，可以使用以下程式碼停止定時器
        clearInterval(gameLoop);
    }
    /*小遊戲*/
})
var strPoints = sessionStorage.getItem('strPoints') || "pointsShop";
var strCate = sessionStorage.getItem('strCate') || "all";
var isSearching = sessionStorage.getItem('isSearching');
var isTranSearching = sessionStorage.getItem("isTranSearching");
var isMG = sessionStorage.getItem("isManager");
var isUser = sessionStorage.getItem("userLogin");

$(function () {
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
    var managerUrl = $("#mainModal").data("url");
    $.ajax({
        type: "GET",
        url: managerUrl
    }).done(function (data) {
        $("#mainModalBody").html(data);
    }).fail(function (err) {
        alert(err.responseText);
    });
    // Managermodal

    /* 管理員登入 */
    $("#home").on("mousedown", function () {
        setTimeout(function () {
            $("#logo").css("animation-name", "logoSpin")
        }, 1000);
        timer = setTimeout(function () {
            $("#logo").css("animation-name", "");
            if (isMG == "true")
            {
                var isMGUrl = $("#logo").data("url");
                //var p = {
                //    key: "true"
                //};
                var form = new FormData();
                //form.append("key", "true");
                sessionStorage.removeItem("isManager");
                $("#mainModalBody").html('<div class="d-flex justify-content-center"><h2 class="m-auto text-gradient">恭喜下班，肝苦人</h2></div>');
                $('#mainModal').modal('show');
                fetch(`${isMGUrl}`, {
                    method: "POST",
                    body: form,
                    /*body: JSON.stringify(p),*/
                    //headers: { 'Content-Type': 'application/json' }
                }).then(function (response) {
                    console.log(response);
                }).catch(function (err) {
                    alert(err);
                });
                setTimeout(function () {
                    location.reload();
                }, 1000);
            }
            else
            {
                $('#mainModal').modal('show');
            }
        }, 3000);
    });
    $("#home").on("mousedup", function () {
        $("#logo").css("animation-name", "")
        clearTimeout(timer);
    });
    /* 管理員登入 */

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
        var gameTargets = $("main").find("div, p, h1, h2, h3, h4, h5, h6").not(".expired-overlay");
        var randEl = Math.floor(Math.random() * gameTargets.length);
        var aniNum = Math.ceil(Math.random() * 4);
        if (gameTargets.eq(randEl).hasClass("position-relative")) {
            sessionStorage.setItem("oriPosRel", "true");
        }
        else {
            gameTargets.eq(randEl).addClass("position-relative");
        }
        gameTargets.eq(randEl).append(`<img id="gameLogo" class="position-absolute p-0" style="width: 30px; height: 30px; z-index: -1;" src=${gameSrc} />`);
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
                        }).catch(err => {
                            alert(err);
                        });
                    }
                }

                toast.show();
                $(this).hide();
                clearInterval(gameLoop);
            }).catch(err => {
                alert(err);
            });
        });
        setTimeout(function () {
            sessionStorage.getItem("oriPosRel") == "true" ? sessionStorage.removeItem("oriPosRel") : $("main").find("#gameLogo").parent().removeClass("position-relative");
            $("main").find("#gameLogo").remove();
        }, 4000);
    }

    if ($("#game").index() != -1 && $(".spinBtn").index() == -1) {
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
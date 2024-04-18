var strPoints = sessionStorage.getItem('strPoints') || "pointsShop";
var strCate = sessionStorage.getItem('strCate') || "all";
var isSearching = sessionStorage.getItem('isSearching');
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

    /*會員登入隱藏活動方*/
    var isLogin = $("#isLogin").find("span");
    if (isLogin.length != 0) {
        //alert("tr");
        $("#Org").hide();
        sessionStorage.setItem("userLogin", "true");
    }
    else {
        //alert("fa");
        $("#Org").show();
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
        $("#logo").css("animation-name","logoSpin")
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
})
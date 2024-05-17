    //路徑
    const vuebase1 = "https://localhost:7271"

    //---------------主要頁面開頭------------------------------
    //活動主頁
    $("#Notification").on("click", (e) => {

    //設置使用者session
    sessionStorage.removeItem("AdminID");
    sessionStorage.setItem("AdminID", "Notification");

    $("#dancingtime").show();
    $.ajax({
        type: "GET",
        url: `${vuebase1}/Admin/_NotificationRecord`
    }).done(function (data) {
        $("#adminBody").html(data);
        $("#dancingtime").hide();
        }).fail(function (err) {
         alert(err.responseText);
          $("#dancingtime").hide();
        });
    })

    //使用者資訊主頁
    $("#AspNetUsers").on("click", (e) => {
    //設置使用者session
    sessionStorage.removeItem("AdminID");
    sessionStorage.setItem("AdminID", "AspNetUsers");

    $("#dancingtime").show();
    $.ajax({
        type: "GET",
        url: `${vuebase1}/Admin/_AspNetUsers`
        }).done(function (data) {
    $("#adminBody").html(data);
    $("#dancingtime").hide();
        }).fail(function (err) {
    alert(err.responseText);
    $("#dancingtime").hide();
        });
    })


    //活動主辦方
    $("#OrganizersAdmin").on("click", (e) => {
    //設置使用者session
    sessionStorage.removeItem("AdminID");
    sessionStorage.setItem("AdminID", "OrganizersAdmin");

    $("#dancingtime").show();
    $.ajax({
        type: "GET",
        url: `${vuebase1}/Admin/_OrganizersAdmin`
        }).done(function (data) {
    $("#adminBody").html(data);
    $("#dancingtime").hide();
        }).fail(function (err) {
    alert(err.responseText);
    $("#dancingtime").hide();
        });
    })

    //進主頁就換
    $(function () {
        const AdminID = sessionStorage.getItem("AdminID");
    $(`#${AdminID}`).trigger("click");
    });

    // //---------------主要頁面結尾------------------------------


    //--------------------圖片即時顯示開始-----
    function previewImage(inputFile, img) {
        if (inputFile.files && inputFile.files[0]) {
            var file = inputFile.files[0];
            var allowType = "image.*";
            if (file.type.match(allowType)) {
                //預覽
                var reader = new FileReader();
                reader.onload = function (e) {
                    img.attr("src", e.target.result);
                    img.attr("title", inputFile.files[0].name);
                };

                reader.readAsDataURL(inputFile.files[0]);
                $(".btn").prop("disabled", false);

            }

            else {
                alert("不支援檔案類型");
                $(".btn").prop("disabled", true);
            }
        }
    }

    $(document).on("change", "#ActivityPhoto", function () {
        previewImage(this, $("#ActivityPhoto").prev());
    });
    //--------------------圖片即時顯示結束-----



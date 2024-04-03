// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
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
})
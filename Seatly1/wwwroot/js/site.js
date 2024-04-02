// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $("#pointsNav").on("mouseenter", function () {
        $(this).addClass("show");
        $(this).attr("aria-expanded","true");
        $("#pointsDropdown").addClass("show");
        $("#pointsDropdown").attr("data-bs-popper","none")
    });
    $("#pointsNav").on("mouseout", function () {
        $(this).removeClass("show");
        $(this).attr("aria-expanded", "false");
        $("#pointsDropdown").removeClass("show");
        $("#pointsDropdown").attr("data-bs-popper","")
    });
})
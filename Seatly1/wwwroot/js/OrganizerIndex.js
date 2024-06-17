$("#o-info").on("click", async function () {
    window.location.href = await '/OrganizerRoute/OrganizerInfo';
});

$("#o-manage").on("click", async function () {
    window.location.href = await '/OrganizerRoute/NotificationRecord';
});

$("#o-active").on("click", async function () {
    window.location.href = await '/Confirm/OrganizerActiveCheckIndex';
});

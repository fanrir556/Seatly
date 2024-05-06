<style>

</style>


<template>
    <NotificationRecordIndex></NotificationRecordIndex>
    <AspUser></AspUser>
    <Organizer></Organizer>
</template>


<script>
    import NotificationRecordIndex from './_NotificationRecord-Index.vue';
    import AspUser from './_AspUser-Index.vue';
    import Organizer from './_Organizer-Index.vue';


    export default {
        name:"adminapp"
        ,components: {
            // 將引入的三個 Vue 單檔元件註冊為這個組件的子組件
            NotificationRecordIndex,
            AspUser,
            Organizer
        },
}
</script>

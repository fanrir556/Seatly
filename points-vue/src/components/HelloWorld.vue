<script setup>
defineProps({
  msg: {
    type: String,
    required: true
  }
})
import { ref } from 'vue'
var img = ref(import.meta.env.VITE_API_SeatlyUrl + '/images/50元折價券.jpg')

var change = () => {
  img.value = import.meta.env.VITE_API_SeatlyUrl + '/images/角落生物.webp'
}

var te = (e) => {
  console.log(e.target.value)
  var imgs = e.target.files[0]
  var reader = new FileReader()
  var allowType = 'image.*'
  // console.log(reader.readAsText($(this).children("td").eq(1).find("input").files[0]));
  if (imgs.type.match(allowType)) {
    reader.readAsDataURL(imgs)
    reader.onload = (e) => {
      // console.log(e.target.result)
      img.value = e.target.result
      // $(this).prev().attr("src", e.target.result);
      // console.log(this.files[0].name)
      // $(this).prev().attr("alt", img.name);
      // $(this).prev().attr("title", img.name);
    }
  }
}
</script>

<template>
  <div class="greetings">
    <h1 class="green">{{ msg }}</h1>
    <h3>
      You’ve successfully created a project with
      <a href="https://vitejs.dev/" target="_blank" rel="noopener">Vite</a> +
      <a href="https://vuejs.org/" target="_blank" rel="noopener">Vue 3</a>.
      <img :src="img" alt="" style="width: 150px; height: 150px" />
      <input type="file" @change="te" />
      <button @click="change">change</button>
    </h3>
  </div>
</template>

<style scoped>
h1 {
  font-weight: 500;
  font-size: 2.6rem;
  position: relative;
  top: -10px;
}

h3 {
  font-size: 1.2rem;
}

.greetings h1,
.greetings h3 {
  text-align: center;
}

@media (min-width: 1024px) {
  .greetings h1,
  .greetings h3 {
    text-align: left;
  }
}
</style>

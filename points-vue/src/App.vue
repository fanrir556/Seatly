<script setup>
import { RouterLink, RouterView } from 'vue-router'
import HelloWorld from './components/HelloWorld.vue'
import { onMounted, onBeforeUnmount, ref } from 'vue'

var logoSrc = ref('')
var dropSmallInterval = ref(null)
var dropLargeInterval = ref(null)
const dropSmallFunction = () => {
  var strRanLeft = Math.random() * 100 + 'vw;'
  var animationTrans = document.querySelector('#animationTrans')
  animationTrans.insertAdjacentHTML(
    'beforeend',
    `<img class="ball" style="left:${strRanLeft};animation-duration: 0.5s;" src="${logoSrc.value}" />`
  )
}
const dropLargeFunction = () => {
  var strRanLeft = Math.random() * 100 + 'vw;'
  var animationTrans = document.querySelector('#animationTrans')
  animationTrans.insertAdjacentHTML(
    'beforeend',
    `<img class="ball" style="left:${strRanLeft}; width:150px; height:150px;animation-duration: 1.0s;" src="${logoSrc.value}" />`
  )
}
const dropXL = () => {
  var animationTrans = document.querySelector('#animationTrans')
  animationTrans.insertAdjacentHTML(
    'beforeend',
    `<img class="ball" style="left:50vw; width:100vh; height:100vh;animation-duration: 1.75s;" src="${logoSrc.value}" />`
  )
}
// 轉場動畫
onMounted(() => {
  axios
    .get(`${import.meta.env.VITE_API_SeatlyUrl}/Points/GetLogo`, {
      responseType: 'blob' // Set the response type to 'blob'
    })
    .then((response) => {
      // Create a URL for the image blob
      const url = URL.createObjectURL(response.data)
      logoSrc.value = url
    })
    .catch((error) => {
      console.error(error)
    })
  var animationTrans = document.querySelector('#animationTrans')
  dropSmallInterval.value = setInterval(dropSmallFunction, 50)
  dropLargeInterval.value = setInterval(dropLargeFunction, 100)
  setTimeout(() => {
    dropXL()
  }, 50)
  setTimeout(() => {
    animationTrans.style.setProperty('animation-name', 'earthquake')
  }, 1800)

  setTimeout(() => {
    clearInterval(dropSmallInterval.value)
    clearInterval(dropLargeInterval.value)
    animationTrans.animate({ top: '200vh' }, 1000, function () {})
  }, 2000)

  setTimeout(() => {
    animationTrans.remove()
  }, 2500)
})
</script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />

    <div class="wrapper">
      <HelloWorld msg="You did it!" />
      <HelloWorld msg="You did it!" />
      <nav>
        <RouterLink to="/">Home</RouterLink>
        <RouterLink to="/about">About</RouterLink>
      </nav>
    </div>
  </header>
  <!-- @* 轉場動畫 *@ -->
  <div id="animationTrans">
    <p
      v-for="i in Array.from({ length: 10 }, (_, j) => j * 10)"
      :key="i"
      class="animationP"
      :style="{ top: i + 'vh' }"
    ></p>
  </div>
  <!-- @* 轉場動畫 *@ -->

  <RouterView />
</template>

<style>
header {
  line-height: 1.5;
  max-height: 100vh;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

nav {
  width: 100%;
  font-size: 12px;
  text-align: center;
  margin-top: 2rem;
}

nav a.router-link-exact-active {
  color: var(--color-text);
}

nav a.router-link-exact-active:hover {
  background-color: transparent;
}

nav a {
  display: inline-block;
  padding: 0 1rem;
  border-left: 1px solid var(--color-border);
}

nav a:first-of-type {
  border: 0;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }

  nav {
    text-align: left;
    margin-left: -1rem;
    font-size: 1rem;

    padding: 1rem 0;
    margin-top: 1rem;
  }
}
/* @* 轉場動畫 *@ */
#animationTrans {
  position: fixed;
  top: 0;
  left: 0;
  margin: 0;
  padding: 0;
  height: 100vh;
  min-width: 100%;
  overflow: hidden;
  z-index: 20;
  background-color: white;
  animation-duration: 0.2s;
}

.animationP {
  position: absolute;
  background-color: rgba(231, 49, 64, 0.5);
  left: 0;
  height: 5vh;
  min-width: 100%;
  z-index: 1;
}

.ball {
  position: absolute;
  width: 50px;
  height: 50px;
  animation: drop 0.8s linear forwards;
}

@keyframes drop {
  0% {
    bottom: 100vh;
    transform: translateX(-50%);
  }

  100% {
    bottom: -50px;
    transform: translateX(-50%);
  }
}

@keyframes earthquake {
  0% {
    left: 20px;
  }
  25% {
    left: -20px;
  }
  50% {
    left: 20px;
  }
  75% {
    left: -20px;
  }
  100% {
    left: 20px;
  }
}
/* @* 轉場動畫 *@ */
</style>

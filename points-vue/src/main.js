import 'bootstrap/dist/css/bootstrap.min.css'
import '../public/theme.min.css'

import 'bootstrap/dist/js/bootstrap.min.js'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
// import router from './router'

const app = createApp(App)

app.use(createPinia())
// app.use(router)
app.config.globalProperties.$axios = window.axios

app.mount('#app')

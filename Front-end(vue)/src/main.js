import './styles/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import vLoading from './components/vLoading'

const app = createApp(App)
const pinia = createPinia()

app.use(router)
app.use(pinia)
app.directive('loading', vLoading)
app.mount('#app')

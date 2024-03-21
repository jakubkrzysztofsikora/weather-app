import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { initializeConfigModule } from './modules'

const app = createApp(App)

// MODULES
const configModule = initializeConfigModule({ httpGet: (url: string) => fetch(url).then(res => res.json()) });

configModule.get().then(console.log);

app.use(router)

app.mount('#app')

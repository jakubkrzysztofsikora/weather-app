import './assets/main.css'
import 'primevue/resources/themes/aura-light-green/theme.css'

import { createApp } from 'vue'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import { initializeConfigModule } from './modules/config'
import { initializeWeatherModule } from './modules/weather'
import initializeRouter from './router'

const app = createApp(App)

// MODULES
const configModule = initializeConfigModule({ httpGet: (url: string) => fetch(url).then(res => res.json()) });
const weatherModule = initializeWeatherModule({ getCities: () => {
    return Promise.resolve([
        { name: 'Dublin'},
    { name: 'Sosnowiec' },
    { name: 'Phoenix' }
    ]);
}
});

app.use(PrimeVue);
app.use(initializeRouter({ weather: weatherModule }));

app.mount('#app')

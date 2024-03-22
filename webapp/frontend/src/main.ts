import './assets/main.css'
import 'primevue/resources/themes/aura-light-green/theme.css'

import { createApp } from 'vue/dist/vue.esm-bundler.js'
import App from './App.vue'
import PrimeVue from 'primevue/config'
import { initializeConfigModule } from './modules/config'
import { initializeWeatherModule } from './modules/weather'
import initializeRouter from './router'
import { City } from './modules/weather/model'
import { capitalize } from './services'

const app = createApp(App)
app.use(PrimeVue)

initializeConfigModule({
  httpGet: (url: string) => fetch(url).then((res) => res.json())
})
  .get()
  .then((config) => {
    const weatherModule = initializeWeatherModule({
      getCities: (search) => fetch(
        `${config.apiUrl}/cities${search ? `?query=${search}` : ""}`
        )
            .then((res) => res.json() as Promise<City[]>)
            .then((cities: City[]) => [...cities.map((city) => ({...city, name: capitalize(city.name) }))]),
      getWeather: (city: string) => fetch(`${config.apiUrl}/weather/${city}`).then((res) => res.json())
    })

    app.use(initializeRouter({ weather: weatherModule }))

    app.mount('#app')
  })
  .catch((e) => {
    console.error(e)
  })

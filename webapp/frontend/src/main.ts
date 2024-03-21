import './assets/main.css'
import 'primevue/resources/themes/aura-light-green/theme.css'

import { createApp } from 'vue/dist/vue.esm-bundler'
import App from './App.vue'
import PrimeVue from 'primevue/config'
import { initializeConfigModule } from './modules/config'
import { initializeWeatherModule } from './modules/weather'
import initializeRouter from './router'
import { City } from './modules/weather/model'
import { Weather } from './modules/weather/model/weather'

const app = createApp(App)
app.use(PrimeVue)

// mocks

const getCitiesMock = (search) => {
  const cities = [{ name: 'Dublin' }, { name: 'Sosnowiec' }, { name: 'Phoenix' }] as City[]
  return search
    ? Promise.resolve(
        cities.filter((city) => city.name.toLowerCase().includes(search.toLowerCase()))
      )
    : Promise.resolve(cities)
}

const getWeatherMock: (city: string) => Promise<Weather> = (city) => {
  return Promise.resolve({
    city: { name: city },
    country: 'Poland',
    description: {
      icon: '//cdn.weatherapi.com/weather/64x64/night/296.png',
      text: 'Clear sky'
    },
    localTimeEpoch: 1626828000,
    sunrise: '05:00',
    sunset: '21:00',
    temperatureCelcius: 25,
    timezoneId: 'Europe/Warsaw'
  } as Weather)
}

// end mocks

initializeConfigModule({
  httpGet: (url: string) => fetch(url).then((res) => res.json())
})
  .get()
  .then((config) => {
    const weatherModule = initializeWeatherModule({
      getCities: getCitiesMock,
      getWeather: (city: string) =>
        new Promise((resolve) => setTimeout(() => resolve(getWeatherMock(city)), 3000)) //fetch(`${config.apiUrl}/weather/${city}`).then((res) => res.json())
    })

    app.use(initializeRouter({ weather: weatherModule }))

    app.mount('#app')
  })
  .catch((e) => {
    console.error(e)
  })

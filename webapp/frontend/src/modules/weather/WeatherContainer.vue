<script setup lang="ts">
import { onMounted, ref, inject, watch } from 'vue'
import AutoComplete from 'primevue/autocomplete'

import type { City } from './model'
import { InjectionKeys } from './ports'
import { Weather } from './model/weather'
import { CurrentWeather } from './components'

const selectedCity = ref<City>()
const filteredCities = ref<City[]>()
const searchTimeout = ref<NodeJS.Timeout | null>()
const loadingCities = ref<boolean>(false)
const loadingWeather = ref<boolean>(false)
const weather = ref<Weather>({
  city: {
    name: ''
  },
  country: '',
  sunrise: '',
  sunset: '',
  temperatureCelcius: 0,
  description: {
    text: '',
    icon: ''
  },
  timezoneId: 'Europe/London',
  localTimeEpoch: 0
} as Weather)
const getCitiesService = inject(InjectionKeys.GetCities)
const getWeatherService = inject(InjectionKeys.GetWeather)

onMounted(() => {
  getCitiesService().then((data) => (filteredCities.value = data))
})

watch(selectedCity, (newValue) => {
  if (newValue) {
    loadingWeather.value = true
    getWeatherService(newValue.name)
      .then((data) => (weather.value = data))
      .finally(() => (loadingWeather.value = false))
  }
})

const search = (event) => {
  if (searchTimeout.value) {
    clearTimeout(searchTimeout.value)
    searchTimeout.value = null
  }

  searchTimeout.value = setTimeout(async () => {
    if (event.query.trim().length) {
      loadingCities.value = true
      filteredCities.value = await getCitiesService(event.query.toLowerCase())
      loadingCities.value = false
    } else {
      loadingCities.value = true
      filteredCities.value = await getCitiesService()
      loadingCities.value = false
    }
  }, 250)
}
</script>

<template>
  <main>
    <div>
      <AutoComplete
        class="city-dropdown"
        v-model="selectedCity"
        optionLabel="name"
        dropdown
        :loading="loadingCities"
        :suggestions="filteredCities"
        @complete="search"
        placeholder="Pick a City"
      />
      <CurrentWeather v-if="selectedCity" :weather="weather" :loading="loadingWeather" />
    </div>
  </main>
</template>
<style scoped>
.city-dropdown {
  width: 100%;
}
</style>

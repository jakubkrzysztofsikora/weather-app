<script setup lang="ts">
import { onMounted, ref, inject, watch } from 'vue'
import AutoComplete, { AutoCompleteCompleteEvent } from 'primevue/autocomplete'

import { EmptyWeather, type City, type Weather } from './model'
import { GetCities, GetMoreInfoLink, GetWeather, InjectionKeys } from './ports'
import { CurrentWeather, WeatherTabs } from './components'

const debounceTime = ref<number>(1000)
const selectedCity = ref<City>()
const filteredCities = ref<City[]>()
const searchTimeout = ref<NodeJS.Timeout | null>()
const loadingCities = ref<boolean>(false)
const loadingWeather = ref<boolean>(false)
const weather = ref<Weather>(EmptyWeather)
const getCitiesService = inject<GetCities>(InjectionKeys.GetCities)
const getWeatherService = inject<GetWeather>(InjectionKeys.GetWeather)
const getMoreInfoLinkService = inject<GetMoreInfoLink>(InjectionKeys.GetMoreInfoLink)

onMounted(() => {
  getCitiesService?.().then((data) => (filteredCities.value = data))
})

watch(selectedCity, (newValue) => {
  if (newValue && filteredCities.value?.includes(newValue)) {
    loadingWeather.value = true
    getWeatherService?.(newValue.name)
      .then((data) => (weather.value = data))
      .finally(() => (loadingWeather.value = false))
  }
})

const search = (event: AutoCompleteCompleteEvent) => {
  if (searchTimeout.value) {
    clearTimeout(searchTimeout.value)
    searchTimeout.value = null
  }

  searchTimeout.value = setTimeout(async () => {
    if (event.query.trim().length) {
      loadingCities.value = true
      filteredCities.value = await getCitiesService?.(event.query.toLowerCase())
      loadingCities.value = false
    } else {
      loadingCities.value = true
      filteredCities.value = await getCitiesService?.()
      loadingCities.value = false
    }
  }, debounceTime.value)
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
        :completeOnFocus="true"
        placeholder="Pick a City"
      />
      <CurrentWeather
        class="current-weather-container"
        v-if="selectedCity && filteredCities?.includes(selectedCity)"
        :weather="weather"
        :loading="loadingWeather"
      />
      <WeatherTabs
        v-if="selectedCity && filteredCities?.includes(selectedCity)"
        :weather="weather"
        :moreInfoLink="getMoreInfoLinkService?.(selectedCity.name)"
        :loading="loadingWeather"
      />
    </div>
  </main>
</template>
<style scoped>
.city-dropdown {
  width: 100%;
}

.current-weather-container {
  margin-top: 2em;
}
</style>

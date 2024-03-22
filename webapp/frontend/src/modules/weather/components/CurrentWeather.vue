<script setup lang="ts">
import Skeleton from 'primevue/skeleton'
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { Weather } from '../model/weather'

const { loading, weather } = defineProps<{ weather: Weather; loading: boolean }>()

const currentDatetime = ref<Date>(weather.localTimeEpoch ? new Date(weather.localTimeEpoch * 1000) : new Date())
const timezone = ref<string>('')

onMounted(() => {
  let timer: NodeJS.Timeout

  timer = setInterval(() => {
    timezone.value = weather.timezoneId
    const currentLocalTime = new Date().getTime() / 1000
      currentDatetime.value = new Date((currentLocalTime + 1) * 1000)
    }, 1000)

  onUnmounted(() => {
    if (timer) {
      clearInterval(timer)
    }
  })
})


//°C
</script>
<template>
  <div class="current-weather">
    <div class="current_weather__child icon">
      <Skeleton v-if="loading || !weather" borderRadius="16px" height="inherit" />
      <img v-else :src="weather.description.icon" :alt="weather.description.text" />
    </div>
    <div class="current_weather__child description">
      <Skeleton v-if="loading || !weather" height="2em" width="10em" />
      <h1 v-else>{{ weather.temperatureCelsius }}°C</h1>
      <Skeleton v-if="loading || !weather" height="1.5em" width="15em" />
      <h2 v-else>{{ currentDatetime?.toLocaleString('default', { timeZone: weather.timezoneId }) }}</h2>
    </div>

  <p v-if="!loading || !weather">
    Last updated: {{ new Date(weather.localTimeEpoch * 1000).toLocaleString() }}
  </p>
  </div>
</template>
<style scoped>
.current-weather {
  display: flex;
  flex-direction: row;
  align-items: center;
  gap: 1em;
  margin: 1em 0;
}

.current_weather__child {
  flex: 1;
  height: 5rem;
}

.icon {
  flex: 0 0 5rem;
  height: 5rem;
}

.icon > * {
  width: 100%;
  height: 100%;
  display: block;
}

.description {
  display: flex;
  flex-direction: column;
  justify-content: center;
  gap: 0.5em;
}
</style>

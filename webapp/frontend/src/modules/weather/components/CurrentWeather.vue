<script setup lang="ts">
import Skeleton from 'primevue/skeleton'
import { ref, watch, onMounted, onUnmounted } from 'vue'
import { Weather } from '../model/weather'

const { loading, weather } = defineProps<{ weather: Weather; loading: boolean }>()

const currentLocalTime = ref<number>(weather.localTimeEpoch)
const datetimeFormatted = ref<string>('')

onMounted(() => {
  const timer = setInterval(() => {
    currentLocalTime.value += 1
  }, 1000)

  watch(currentLocalTime, () => {
    const date = new Date(currentLocalTime.value * 1000)
    const formatter = new Intl.DateTimeFormat('default', {
      timeStyle: 'medium',
      dateStyle: 'short',
      timeZone: weather.timezoneId
    })
    datetimeFormatted.value = formatter.format(date)
  })

  onUnmounted(() => {
    clearInterval(timer)
  })
})

//°C
</script>
<template>
  <div class="current-weather">
    <div class="current_weather__child icon">
      <Skeleton v-if="loading" borderRadius="16px" height="inherit" />
      <img v-else :src="weather.description.icon" :alt="weather.description.text" />
    </div>
    <div class="current_weather__child description">
      <Skeleton v-if="loading" height="2em" width="10em" />
      <h1 v-else>{{ weather.temperatureCelcius }}°C</h1>
      <Skeleton v-if="loading" height="1.5em" width="15em" />
      <h2 v-else>{{ datetimeFormatted }}</h2>
    </div>
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

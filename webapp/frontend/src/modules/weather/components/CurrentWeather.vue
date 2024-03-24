<script setup lang="ts">
import Skeleton from 'primevue/skeleton'
import { ref, onMounted, onUnmounted, watch } from 'vue'
import { Weather } from '../model/weather'

const { loading, weather } = defineProps<{ weather: Weather; loading: boolean }>()

const currentDatetime = ref<Date>(
  weather.localTimeEpoch ? new Date(weather.localTimeEpoch * 1000) : new Date()
)
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
    <div class="current-weather__child">
      <div class="icon">
        <Skeleton v-if="loading || !weather" borderRadius="16px" height="inherit" />
        <img v-else :src="weather.description.icon" :alt="weather.description.text" />
      </div>
      <div class="description">
        <Skeleton v-if="loading || !weather" height="2em" width="10em" />
        <h1 v-else>{{ weather.temperatureCelsius }}°C</h1>
        <Skeleton v-if="loading || !weather" height="1.5em" width="15em" />
        <h2 v-else>
          {{ currentDatetime?.toLocaleString('default', { timeZone: weather.timezoneId }) }}
        </h2>
      </div>
    </div>

    <p class="slightly-less-important" v-if="!loading || !weather">
      Last updated: {{ new Date(weather.localTimeEpoch * 1000).toLocaleString() }}
    </p>
  </div>
</template>
<style scoped>
.current-weather {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: space-between;
  gap: 1em;
  margin: 1em 0;

  @media screen and (max-width: 1024px) {
    flex-direction: column;
  }

  .current-weather__child {
    flex: 1;
    height: 5rem;
    display: flex;
    align-items: center;
    gap: 1em;
  }

  .icon {
    flex: 0 0 5rem;
    height: 5rem;

    & > * {
      width: 100%;
      height: 100%;
      display: block;
    }
  }

  .description {
    display: flex;
    flex-direction: column;
    justify-content: center;
    gap: 0.5em;
  }

  .slightly-less-important {
    color: #666;
  }
}
</style>

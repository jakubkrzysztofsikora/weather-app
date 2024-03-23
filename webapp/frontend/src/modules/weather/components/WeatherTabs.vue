<script setup lang="ts">
import TabView, { TabViewClickEvent } from 'primevue/tabview'
import TabPanel from 'primevue/tabpanel'
import Skeleton from 'primevue/skeleton'
import Button from 'primevue/button'

import { timeStringToNumber, numberToTimeSpan } from '../services'
import { Weather } from '../model/weather'
import { ref } from 'vue'

const { weather, selectedCityName, loading } = defineProps<{
  weather: Weather
  selectedCityName: string
  loading: boolean
}>()

const ASTRONOMY_TAB_INDEX = 0
const MORE_WEATHER_TAB_INDEX = 1
const moreLink = `https://www.weatherapi.com/weather/q/${selectedCityName}`
const currentTab = ref<number>(ASTRONOMY_TAB_INDEX)
const tabs = ref<
  (weather: Weather) => { label: string; index: number; content: string; link?: string }[]
>((weather) => [
  {
    label: 'Day and night',
    index: ASTRONOMY_TAB_INDEX,
    content: `Today the sun will rise at ${weather.sunrise} and set at ${weather.sunset}, hence
            will be up for
            ${numberToTimeSpan(
              timeStringToNumber(weather.sunset) - timeStringToNumber(weather.sunrise)
            )}.`
  },
  {
    label: 'More',
    index: MORE_WEATHER_TAB_INDEX,
    content: "You're being redirected to the website with full weather information.",
    link: moreLink
  }
])

const onTabClick = (e: TabViewClickEvent) => {
  if (e.index !== MORE_WEATHER_TAB_INDEX) {
    return
  }

  window.open(moreLink, '_blank')
}
</script>
<template>
  <TabView v-model:activeIndex="currentTab" @tab-click="onTabClick">
    <TabPanel v-for="tab in tabs(weather)" :key="tab.index" :header="tab.label">
      <p v-if="loading">
        <Skeleton class="skeleton" width="100%"></Skeleton>
        <Skeleton class="skeleton" width="75%"></Skeleton>
      </p>
      <p v-else>{{ tab.content }}</p>
      <a :href="tab.link" target="_blank" v-if="tab.link">
        <Button class="more" label="More" />
      </a>
    </TabPanel>
  </TabView>
</template>
<style scoped>
.skeleton {
  margin-bottom: 0.5em;
  height: 1em;
}

.more {
  margin-top: 1em;
}
</style>

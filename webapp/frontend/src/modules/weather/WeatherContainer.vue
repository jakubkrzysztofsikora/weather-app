<script setup lang="ts">
import { onMounted, ref } from "vue";
import AutoComplete from 'primevue/autocomplete';


import type { City } from "./model"

const selectedCity = ref<City>();
const filteredCities = ref<City[]>();
const searchTimeout = ref<NodeJS.Timeout | null>();
const loadingCities = ref<boolean>(false);

const props = defineProps({
    getCities: {
        type: Function,
        required: true
    }
});

console.log({props})
onMounted(() => {
    props.getCities().then((data) => (filteredCities.value = data));
});


const search = (event) => {
    if (searchTimeout.value) {
        clearTimeout(searchTimeout.value);
        searchTimeout.value = null;
    }

    searchTimeout.value = setTimeout(async () => {
        if (event.query.trim().length) {
            loadingCities.value = true;
            filteredCities.value = await props.getCities(event.query.toLowerCase());
            loadingCities.value = false;
        }
    }, 250);
}
</script>

<template>
  <main>
    <div>
        <AutoComplete class="city-dropdown" v-model="selectedCity" optionLabel="name" dropdown :loading="loadingCities" :suggestions="filteredCities" @complete="search" placeholder="Pick a City" /> 
    </div>
  </main>
</template>
<style scoped>
.city-dropdown {
    width: 100%;
}
</style>

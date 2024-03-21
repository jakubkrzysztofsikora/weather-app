import { defineComponent } from "vue";
import WeatherContainer from "./WeatherContainer.vue"
import { GetCities } from "./ports";

export const initializeWeatherModule = (services: { getCities: GetCities}) => {
    const props = { getCities: services.getCities };
    return { container: defineComponent({
        components: { WeatherContainer },
        props,
        template: '<WeatherContainer v-bind="$props" />'
    }) };
}
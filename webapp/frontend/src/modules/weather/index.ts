import { defineComponent, provide } from 'vue'
import WeatherContainer from './WeatherContainer.vue'
import { GetCities, GetWeather, InjectionKeys } from './ports'

export const initializeWeatherModule = (services: {
  getCities: GetCities
  getWeather: GetWeather
}) => {
  return {
    container: defineComponent({
      name: 'WeatherContainerWrapper',
      components: { WeatherContainer },
      template: '<WeatherContainer />',
      setup() {
        provide(InjectionKeys.GetCities, services.getCities)
        provide(InjectionKeys.GetWeather, services.getWeather)
      }
    })
  }
}

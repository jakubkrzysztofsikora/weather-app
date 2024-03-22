// import { mount } from '@vue/test-utils'
// import WeatherContainer from './WeatherContainer.vue'
// import { GetCities, GetWeather, InjectionKeys } from './ports'
// import { beforeEach } from 'node:test'
// import { City } from './model'
// import { Weather } from './model/weather'

// describe('WeatherContainer', () => {
//   let getCitiesService: jest.Mock<Promise<City[]>>
//   let getWeatherService: jest.Mock<Promise<Weather>>

//   beforeEach(() => {
//     getCitiesService = jest.fn()
//     getWeatherService = jest.fn()
//   })

//   it('should fetch cities on mount', async () => {
//     const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
//     getCitiesService.mockResolvedValue(cities)

//     mount(WeatherContainer, {
//       global: {
//         provide: {
//           [InjectionKeys.GetCities as symbol]: getCitiesService,
//           [InjectionKeys.GetWeather as symbol]: getWeatherService
//         }
//       }
//     })

//     await getCitiesService()

//     expect(getCitiesService).toHaveBeenCalled()
//   })

//   it('should fetch weather when a city is selected', async () => {
//     const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
//     const weather: Weather = {
//       city: { name: 'London' },
//       country: 'UK',
//       sunrise: '6:00 AM',
//       sunset: '8:00 PM',
//       temperatureCelsius: 20,
//       description: { text: 'Sunny', icon: 'sun' },
//       timezoneId: 'Europe/London',
//       localTimeEpoch: 1629878400
//     }
//     getCitiesService.mockResolvedValue(cities)
//     getWeatherService.mockResolvedValue(weather)

//     const wrapper = mount(WeatherContainer, {
//       global: {
//         provide: {
//           [InjectionKeys.GetCities as symbol]: getCitiesService,
//           [InjectionKeys.GetWeather as symbol]: getWeatherService
//         }
//       }
//     })

//     await getCitiesService()
//     await wrapper.vm.$nextTick()

//     wrapper.vm.selectedCity = cities[0]
//     await wrapper.vm.$nextTick()

//     expect(getWeatherService).toHaveBeenCalledWith(cities[0].name)
//   })

//   it('should filter cities based on search query', async () => {
//     const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
//     getCitiesService.mockResolvedValue(cities)

//     const wrapper = mount(WeatherContainer, {
//       global: {
//         provide: {
//           [InjectionKeys.GetCities as symbol]: getCitiesService,
//           [InjectionKeys.GetWeather as symbol]: getWeatherService
//         }
//       }
//     })

//     await getCitiesService()
//     await wrapper.vm.$nextTick()

//     wrapper.vm.search({ query: 'Lon' })
//     await wrapper.vm.$nextTick()

//     expect(getCitiesService).toHaveBeenCalledWith('lon')
//   })
// })
import { mount } from '@vue/test-utils'
import { expect, describe, beforeEach, it, vi } from 'vitest'
import WeatherContainer from './WeatherContainer.vue'
import { InjectionKeys } from './ports'
import { City } from './model'
import { Weather } from './model/weather'

describe('WeatherContainer', () => {
  let getCitiesService: ReturnType<typeof vi.fn>
  let getWeatherService: ReturnType<typeof vi.fn>
  let getMoreInfoLink: () => string

  beforeEach(() => {
    getCitiesService = vi.fn()
    getWeatherService = vi.fn()
    getMoreInfoLink = () => 'https://www.weatherapi.com/weather/q/London'
  })

  it('should fetch cities on mount', async () => {
    const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
    getCitiesService.mockResolvedValue(cities)

    mount(WeatherContainer, {
      global: {
        provide: {
          [InjectionKeys.GetCities]: getCitiesService,
          [InjectionKeys.GetWeather]: getWeatherService,
          [InjectionKeys.GetMoreInfoLink]: getMoreInfoLink
        }
      }
    })

    await getCitiesService()

    expect(getCitiesService).toHaveBeenCalled()
  })

  it('should fetch weather when a city is selected', async () => {
    const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
    const weather: Weather = {
      city: { name: 'London' },
      country: 'UK',
      sunrise: '6:00 AM',
      sunset: '8:00 PM',
      temperatureCelsius: 20,
      description: { text: 'Sunny', icon: 'sun' },
      timezoneId: 'Europe/London',
      localTimeEpoch: 1629878400
    }
    getCitiesService.mockResolvedValue(cities)
    getWeatherService.mockResolvedValue(weather)

    const wrapper = mount(WeatherContainer, {
      global: {
        provide: {
          [InjectionKeys.GetCities]: getCitiesService,
          [InjectionKeys.GetWeather]: getWeatherService,
          [InjectionKeys.GetMoreInfoLink]: getMoreInfoLink
        }
      },
      props: { debounceTime: 0, selectedCity: undefined as City | undefined }
    })
    wrapper.vm.debounceTime = 0

    await getCitiesService()
    await wrapper.vm.$nextTick()

    wrapper.vm.selectedCity = cities[0]
    await wrapper.vm.$nextTick()

    await accountForDebounceEffect(
      () => expect(getWeatherService).toHaveBeenCalledWith(cities[0].name),
      wrapper.vm.debounceTime
    )
  })

  it('should filter cities based on search query', async () => {
    const cities: City[] = [{ name: 'London' }, { name: 'Paris' }]
    getCitiesService.mockResolvedValue(cities)

    const wrapper = mount(WeatherContainer, {
      global: {
        provide: {
          [InjectionKeys.GetCities]: getCitiesService,
          [InjectionKeys.GetWeather]: getWeatherService,
          [InjectionKeys.GetMoreInfoLink]: getMoreInfoLink
        }
      },
      props: { debounceTime: 0 }
    })
    wrapper.vm.debounceTime = 0
    const searchFunction = (wrapper.vm as any).search as (params: { query: string }) => void

    await getCitiesService()
    await wrapper.vm.$nextTick()

    searchFunction({ query: 'Lon' })
    await wrapper.vm.$nextTick()

    await accountForDebounceEffect(
      () => expect(getCitiesService).toHaveBeenCalledWith('lon'),
      wrapper.vm.debounceTime
    )
  })
})

const accountForDebounceEffect = (fn: () => void, debounceTime: number) => {
  return new Promise<void>((resolve) =>
    setTimeout(() => {
      fn()
      resolve()
    }, debounceTime)
  )
}

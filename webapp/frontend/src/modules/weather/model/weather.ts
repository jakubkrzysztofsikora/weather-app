import { City } from './city'

export type Weather = {
  city: City
  country: string
  localTimeEpoch: number
  timezoneId: string
  sunset: string
  sunrise: string
  temperatureCelsius: number
  description: {
    text: string
    icon: string
  }
}

export const EmptyWeather: Weather = {
  city: {
    name: ''
  },
  country: '',
  localTimeEpoch: 0,
  timezoneId: 'Europe/London',
  sunset: '',
  sunrise: '',
  temperatureCelsius: 0,
  description: {
    text: '',
    icon: ''
  }
}

import { City } from './city'

export type Weather = {
  city: City
  country: string
  localTimeEpoch: number
  timezoneId: string
  sunset: string
  sunrise: string
  temperatureCelcius: number
  description: {
    text: string
    icon: string
  }
}

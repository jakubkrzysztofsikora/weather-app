import type { Weather } from '../model/weather'
export type GetWeather = (city: string) => Promise<Weather>

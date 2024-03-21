import { getConfig } from './get-config'
import type { Config } from './model'
import type { HttpGet } from './ports'

export const initializeConfigModule = (services: {
  httpGet: HttpGet
}): { get: () => Promise<Config> } => {
  return {
    get: async () => {
      console.log(import.meta.env)
      return import.meta.env.MODE === 'development' && import.meta.env.DEV === true
        ? { apiUrl: import.meta.env.VITE_API_URL }
        : getConfig(services.httpGet)
    }
  }
}

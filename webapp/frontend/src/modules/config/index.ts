import { getConfig } from "./get-config";
import type { Config } from "./model";
import type { HttpGet } from "./ports";

export const initializeConfigModule = (services: { httpGet: HttpGet }): { get: () => Promise<Config> } => {
    return {
        get: async () => {
            return getConfig(services.httpGet);
        }
    }
}
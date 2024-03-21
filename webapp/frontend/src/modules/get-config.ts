import type { Config } from "./model";
import type { HttpGet } from "./ports";

export const getConfig: (httpGet: HttpGet) => Promise<Config> = async (httpGet) => {
    return httpGet<Config>('/config');
}
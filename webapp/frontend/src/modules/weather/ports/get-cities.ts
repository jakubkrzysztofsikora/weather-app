import { City } from "../model";

export type GetCities = (search?: string) => Promise<City[]>;
import { Country } from "./country";

export interface CountriesResponse {
    pageNumber: number
    pageSize: number
    totalRecords: number
    totalPages: number
    countries: Country[]
  }
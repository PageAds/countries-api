import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { Country } from '../models/country';
import { CountriesResponse } from '../models/countriesResponse';

@Injectable({
  providedIn: 'root'
})
export class MockCountriesApiService {
  public getCountries(): Observable<CountriesResponse> {

    var countries: Country[] = [
      { name: 'United Kingdom', flagUrl: 'https://flagcdn.com/w320/gb.png', population: 0, timeZones:[], currencies: [], languages: [], capitalCities: [], borders: [] },
      { name: 'Ireland', flagUrl: 'https://flagcdn.com/w320/ca.png', population: 0, timeZones:[], currencies: [], languages: [], capitalCities: [], borders: [] },
      { name: 'France', flagUrl: 'https://flagcdn.com/w320/fr.png', population: 0, timeZones:[], currencies: [], languages: [], capitalCities: [], borders: [] }
    ]

    var countriesResponse: CountriesResponse = { 
      pageNumber: 1,
      pageSize: 3,
      totalRecords: 3,
      totalPages: 1,
      countries: countries
    }

    return of(countriesResponse);
  }
}

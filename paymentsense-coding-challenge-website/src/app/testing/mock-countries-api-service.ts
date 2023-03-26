import { Injectable } from '@angular/core';
import { of, Observable } from 'rxjs';
import { Country } from '../models/country';

@Injectable({
  providedIn: 'root'
})
export class MockCountriesApiService {
  public getCountries(): Observable<Country[]> {

    var countries: Country[] = [
      { name: 'United Kingdom' },
      { name: 'Ireland' },
      { name: 'France' }
    ]

    return of(countries);
  }
}

import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Country } from '../models/country';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CountriesApiService {
  private apiUrl = environment.countriesApiUrl

  constructor(private httpClient: HttpClient) {}
  
  public getCountries() : Observable<Country[]>
  {
    return this.httpClient.get<Country[]>(`${this.apiUrl}/countries`, { responseType: 'json' });
  }
}

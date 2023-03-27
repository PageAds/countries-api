import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { CountriesResponse } from '../models/countriesResponse';

@Injectable({
  providedIn: 'root'
})
export class CountriesApiService {
  private apiUrl = environment.countriesApiUrl

  constructor(private httpClient: HttpClient) {}
  
  public getCountries(pageNumber: number, pageSize: number) : Observable<CountriesResponse>
  {
    return this.httpClient.get<CountriesResponse>(`${this.apiUrl}/countries?pageNumber=${pageNumber}&pageSize=${[pageSize]}`, { responseType: 'json' });
  }
}

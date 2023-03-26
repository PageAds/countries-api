import { Component } from '@angular/core';
import { CountriesApiService } from '../services/countries-api-service'
import { Country } from '../models/country'
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-countries-list',
  templateUrl: './countries-list.component.html',
  styleUrls: ['./countries-list.component.scss']
})
export class CountriesListComponent {
  public countries: Country[] = [];
  public countriesApiRequestCompleted: boolean = false;

  constructor(
    private countriesApiService: CountriesApiService,
    private snackBar: MatSnackBar) {}
    
  ngOnInit() {
    this.getCountries();
  }

  getCountries() {
    this.countriesApiService.getCountries()
    .subscribe({
      next: async (countries) => {
        this.countries = countries;
        this.countriesApiRequestCompleted = true;
      },
      error: (err) => {
        console.error('Error occurred when retrieving countries via API');
        console.error(err);
        this.countriesApiRequestCompleted = true;
        this.snackBar.open('Unable to retrieve countries', 'X', {
          horizontalPosition: 'center',
          verticalPosition: 'top'
        });
      }
    })
  }
}

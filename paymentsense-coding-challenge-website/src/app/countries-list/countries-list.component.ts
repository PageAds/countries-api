import { Component } from '@angular/core';
import { CountriesApiService } from '../services'
import { Country } from '../models/country'
import { MatSnackBar } from '@angular/material/snack-bar';
import { PageEvent } from '@angular/material/paginator';
import { CountryDetailsDialogComponent } from '../country-details-dialog/country-details-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-countries-list',
  templateUrl: './countries-list.component.html',
  styleUrls: ['./countries-list.component.scss']
})
export class CountriesListComponent {
  public countries: Country[] = [];
  public countriesApiRequestCompleted: boolean = false;
  public countriesApiRequestSucceeded: boolean = false;
  public displayedColumns: string[] = ['name', 'flag'];
  public pageNumber: number = 0;
  public pageSize: number = 5;
  public totalRecords: number = 0;
  public totalPages: number = 0;
  public pageEvent: PageEvent;

  constructor(
    private countriesApiService: CountriesApiService,
    private snackBar: MatSnackBar,
    private dialog: MatDialog) {
      this.pageEvent = null!;
    }
    
  ngOnInit() {
    this.getCountries(null!);
  }

  getCountries(event: PageEvent) {
    this.pageEvent = event;
    this.countriesApiService.getCountries(event == null ? 1 : event.pageIndex + 1, event == null ? this.pageSize : event.pageSize)
    .subscribe({
      next: async (countriesResponse) => {
        this.countries = countriesResponse.countries;
        this.totalRecords = countriesResponse.totalRecords;
        this.totalPages = countriesResponse.totalPages;
        this.countriesApiRequestCompleted = true;
        this.countriesApiRequestSucceeded = true;
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

  openDialog(country: Country) {
    this.dialog.open(CountryDetailsDialogComponent, {
      data: country
    });
  }
}

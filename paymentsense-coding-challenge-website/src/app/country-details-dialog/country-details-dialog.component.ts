import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Country } from '../models/country';

@Component({
  selector: 'app-country-details-dialog',
  templateUrl: './country-details-dialog.component.html',
  styleUrls: ['./country-details-dialog.component.scss']
})
export class CountryDetailsDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: Country) {}
}

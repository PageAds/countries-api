import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Country } from '../models/country';
import { CountryDetailsDialogComponent } from './country-details-dialog.component';

describe('CountryDetailsDialogComponent', () => {
  let component: CountryDetailsDialogComponent;
  let fixture: ComponentFixture<CountryDetailsDialogComponent>;

  const country: Country = { name: 'United Kingdom', flagUrl: 'https://flagcdn.com/w320/gb.png', population: 67215293, timeZones:['UTC'], currencies: ['GBP'], languages: ['English'], capitalCities: ['London'], borders: ['Ireland'] };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CountryDetailsDialogComponent ],
      providers: [ 
        { provide: MAT_DIALOG_DATA, useValue: country }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CountryDetailsDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render dialog containing country details', () => {
    const fixture = TestBed.createComponent(CountryDetailsDialogComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('h1').textContent).toBe(`${country.name} details`);
    expect(compiled.querySelector('#country-detail-population').textContent).toBe(`Population: ${country.population}`);
    expect(compiled.querySelector('#country-detail-time-zones li').textContent).toBe(`${country.timeZones}`);
    expect(compiled.querySelector('#country-detail-currencies li').textContent).toBe(`${country.currencies}`);
    expect(compiled.querySelector('#country-detail-languages li').textContent).toBe(`${country.languages}`);
    expect(compiled.querySelector('#country-detail-capital-cities li').textContent).toBe(`${country.capitalCities}`);
    expect(compiled.querySelector('#country-detail-borders li').textContent).toBe(`${country.borders}`);
  });
});

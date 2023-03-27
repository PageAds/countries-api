import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CountriesListComponent } from './countries-list.component';
import { CountriesApiService } from '../services/countries-api-service';
import { MockCountriesApiService } from '../testing/mock-countries-api-service';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatDialogModule } from '@angular/material/dialog';

describe('CountriesListComponent', () => {
  let component: CountriesListComponent;
  let fixture: ComponentFixture<CountriesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ 
        BrowserAnimationsModule,
        MatListModule,
        MatTableModule,
        MatPaginatorModule,
        MatDialogModule
       ],
      declarations: [ CountriesListComponent ],
      providers: [
        { provide: CountriesApiService, useClass: MockCountriesApiService },
        { provide: MatSnackBar, useClass: MatSnackBar },
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CountriesListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render table containing country names', () => {
    const fixture = TestBed.createComponent(CountriesListComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('tbody tr:nth-of-type(1) .mat-column-name').textContent).toBe('United Kingdom');
    expect(compiled.querySelector('tbody tr:nth-of-type(2) .mat-column-name').textContent).toBe('Ireland');
    expect(compiled.querySelector('tbody tr:nth-of-type(3) .mat-column-name').textContent).toBe('France');
  });

  it('should render table containing country flags', () => {
    const fixture = TestBed.createComponent(CountriesListComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('tbody tr:nth-of-type(1) .mat-column-flag img').src).toBe('https://flagcdn.com/w320/gb.png');
    expect(compiled.querySelector('tbody tr:nth-of-type(2) .mat-column-flag img').src).toBe('https://flagcdn.com/w320/ca.png');
    expect(compiled.querySelector('tbody tr:nth-of-type(3) .mat-column-flag img').src).toBe('https://flagcdn.com/w320/fr.png');
  });
});

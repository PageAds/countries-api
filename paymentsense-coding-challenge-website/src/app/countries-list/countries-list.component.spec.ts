import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CountriesListComponent } from './countries-list.component';
import { CountriesApiService } from '../services/countries-api-service';
import { MockCountriesApiService } from '../testing/mock-countries-api-service';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar } from '@angular/material/snack-bar';

describe('CountriesListComponent', () => {
  let component: CountriesListComponent;
  let fixture: ComponentFixture<CountriesListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ MatListModule ],
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

  it('should render list of country names', () => {
    const fixture = TestBed.createComponent(CountriesListComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('mat-list-item:nth-of-type(1)').textContent).toBe('United Kingdom');
    expect(compiled.querySelector('mat-list-item:nth-of-type(2)').textContent).toBe('Ireland');
    expect(compiled.querySelector('mat-list-item:nth-of-type(3)').textContent).toBe('France');
  });
});

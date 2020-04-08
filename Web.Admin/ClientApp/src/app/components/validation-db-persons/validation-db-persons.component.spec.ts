import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbPersonsComponent } from './validation-db-persons.component';

describe('ValidationDbPersonsComponent', () => {
  let component: ValidationDbPersonsComponent;
  let fixture: ComponentFixture<ValidationDbPersonsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbPersonsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbPersonsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

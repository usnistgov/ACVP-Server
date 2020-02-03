import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbValidationComponent } from './validation-db-validation.component';

describe('ValidationDbValidationComponent', () => {
  let component: ValidationDbValidationComponent;
  let fixture: ComponentFixture<ValidationDbValidationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbValidationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbValidationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

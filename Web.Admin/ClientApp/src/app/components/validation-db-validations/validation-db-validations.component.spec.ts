import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbValidationsComponent } from './validation-db-validations.component';

describe('ValidationDbValidationsComponent', () => {
  let component: ValidationDbValidationsComponent;
  let fixture: ComponentFixture<ValidationDbValidationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbValidationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbValidationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

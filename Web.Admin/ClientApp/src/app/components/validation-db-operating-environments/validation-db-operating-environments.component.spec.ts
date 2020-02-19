import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbOperatingEnvironmentsComponent } from './validation-db-operating-environments.component';

describe('ValidationDbOperatingEnvironmentsComponent', () => {
  let component: ValidationDbOperatingEnvironmentsComponent;
  let fixture: ComponentFixture<ValidationDbOperatingEnvironmentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbOperatingEnvironmentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbOperatingEnvironmentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbOrganizationComponent } from './validation-db-organization.component';

describe('ValidationDbOrganizationComponent', () => {
  let component: ValidationDbOrganizationComponent;
  let fixture: ComponentFixture<ValidationDbOrganizationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbOrganizationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbOrganizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

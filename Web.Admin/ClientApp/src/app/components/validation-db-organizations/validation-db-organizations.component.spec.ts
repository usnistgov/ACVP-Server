import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbOrganizationsComponent } from './validation-db-organizations.component';

describe('ValidationDbOrganizationsComponent', () => {
  let component: ValidationDbOrganizationsComponent;
  let fixture: ComponentFixture<ValidationDbOrganizationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbOrganizationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbOrganizationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

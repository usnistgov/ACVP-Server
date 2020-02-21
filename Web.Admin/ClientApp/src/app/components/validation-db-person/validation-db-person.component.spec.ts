import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbPersonComponent } from './validation-db-person.component';

describe('ValidationDbPersonComponent', () => {
  let component: ValidationDbPersonComponent;
  let fixture: ComponentFixture<ValidationDbPersonComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbPersonComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbPersonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

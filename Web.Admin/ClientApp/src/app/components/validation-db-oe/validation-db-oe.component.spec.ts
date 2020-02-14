import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbOeComponent } from './validation-db-oe.component';

describe('ValidationDbOeComponent', () => {
  let component: ValidationDbOeComponent;
  let fixture: ComponentFixture<ValidationDbOeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbOeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbOeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

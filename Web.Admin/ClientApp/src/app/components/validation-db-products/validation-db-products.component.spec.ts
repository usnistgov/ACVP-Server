import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbProductsComponent } from './validation-db-products.component';

describe('ValidationDbProductsComponent', () => {
  let component: ValidationDbProductsComponent;
  let fixture: ComponentFixture<ValidationDbProductsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbProductsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbProductsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbProductComponent } from './validation-db-product.component';

describe('ValidationDbProductComponent', () => {
  let component: ValidationDbProductComponent;
  let fixture: ComponentFixture<ValidationDbProductComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbProductComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbProductComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

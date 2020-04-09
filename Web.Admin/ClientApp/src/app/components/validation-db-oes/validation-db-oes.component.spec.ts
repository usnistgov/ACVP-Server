import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbOEsComponent } from './validation-db-oes.component';

describe('ValidationDbOEsComponent', () => {
  let component: ValidationDbOEsComponent;
  let fixture: ComponentFixture<ValidationDbOEsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbOEsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbOEsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

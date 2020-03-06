import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbDependencyComponent } from './validation-db-dependency.component';

describe('ValidationDbDependencyComponent', () => {
  let component: ValidationDbDependencyComponent;
  let fixture: ComponentFixture<ValidationDbDependencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbDependencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbDependencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

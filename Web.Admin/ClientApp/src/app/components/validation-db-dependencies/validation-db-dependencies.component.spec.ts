import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidationDbDependenciesComponent } from './validation-db-dependencies.component';

describe('ValidationDbDependenciesComponent', () => {
  let component: ValidationDbDependenciesComponent;
  let fixture: ComponentFixture<ValidationDbDependenciesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidationDbDependenciesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidationDbDependenciesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

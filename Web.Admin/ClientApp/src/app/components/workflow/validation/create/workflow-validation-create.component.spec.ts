import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowValidationCreateComponent } from './workflow-validation-create.component';

describe('WorkflowValidationCreateComponent', () => {
  let component: WorkflowValidationCreateComponent;
  let fixture: ComponentFixture<WorkflowValidationCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowValidationCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowValidationCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

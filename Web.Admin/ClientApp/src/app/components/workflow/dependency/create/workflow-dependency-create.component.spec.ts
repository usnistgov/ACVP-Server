import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowDependencyCreateComponent } from './workflow-dependency-create.component';

describe('WorkflowDependencyCreateComponent', () => {
  let component: WorkflowDependencyCreateComponent;
  let fixture: ComponentFixture<WorkflowDependencyCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowDependencyCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowDependencyCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

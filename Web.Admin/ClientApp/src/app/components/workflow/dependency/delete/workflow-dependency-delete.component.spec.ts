import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowDependencyDeleteComponent } from './workflow-dependency-delete.component';

describe('WorkflowDependencyDeleteComponent', () => {
  let component: WorkflowDependencyDeleteComponent;
  let fixture: ComponentFixture<WorkflowDependencyDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowDependencyDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowDependencyDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

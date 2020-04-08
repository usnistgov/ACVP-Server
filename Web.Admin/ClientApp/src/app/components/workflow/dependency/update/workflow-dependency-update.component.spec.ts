import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowDependencyUpdateComponent } from './workflow-dependency-update.component';

describe('WorkflowDependencyUpdateComponent', () => {
  let component: WorkflowDependencyUpdateComponent;
  let fixture: ComponentFixture<WorkflowDependencyUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowDependencyUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowDependencyUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

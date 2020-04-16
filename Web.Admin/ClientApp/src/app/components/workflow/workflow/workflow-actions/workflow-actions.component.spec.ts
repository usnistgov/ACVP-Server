import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowActionsComponent } from './workflow-actions.component';

describe('WorkflowActionsComponent', () => {
  let component: WorkflowActionsComponent;
  let fixture: ComponentFixture<WorkflowActionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowActionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowActionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

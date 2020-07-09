import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowsMultiApproveComponent } from './workflows-multi-approve.component';

describe('WorkflowsMultiApproveComponent', () => {
  let component: WorkflowsMultiApproveComponent;
  let fixture: ComponentFixture<WorkflowsMultiApproveComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowsMultiApproveComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowsMultiApproveComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

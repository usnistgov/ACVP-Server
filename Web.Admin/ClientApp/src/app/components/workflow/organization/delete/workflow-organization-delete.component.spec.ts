import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOrganizationDeleteComponent } from './workflow-organization-delete.component';

describe('WorkflowOrganizationDeleteComponent', () => {
  let component: WorkflowOrganizationDeleteComponent;
  let fixture: ComponentFixture<WorkflowOrganizationDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOrganizationDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOrganizationDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

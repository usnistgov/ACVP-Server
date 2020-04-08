import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOrganizationCreateComponent } from './workflow-organization-create.component';

describe('WorkflowOrganizationCreateComponent', () => {
  let component: WorkflowOrganizationCreateComponent;
  let fixture: ComponentFixture<WorkflowOrganizationCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOrganizationCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOrganizationCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOrganizationUpdateComponent } from './workflow-organization-update.component';

describe('WorkflowOrganizationUpdateComponent', () => {
  let component: WorkflowOrganizationUpdateComponent;
  let fixture: ComponentFixture<WorkflowOrganizationUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOrganizationUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOrganizationUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

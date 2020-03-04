import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOeCreateComponent } from './workflow-oe-create.component';

describe('WorkflowOeCreateComponent', () => {
  let component: WorkflowOeCreateComponent;
  let fixture: ComponentFixture<WorkflowOeCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOeCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOeCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

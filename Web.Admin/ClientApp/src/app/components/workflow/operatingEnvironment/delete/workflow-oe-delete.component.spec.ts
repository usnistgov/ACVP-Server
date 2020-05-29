import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOeDeleteComponent } from './workflow-oe-delete.component';

describe('WorkflowOeDeleteComponent', () => {
  let component: WorkflowOeDeleteComponent;
  let fixture: ComponentFixture<WorkflowOeDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOeDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOeDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

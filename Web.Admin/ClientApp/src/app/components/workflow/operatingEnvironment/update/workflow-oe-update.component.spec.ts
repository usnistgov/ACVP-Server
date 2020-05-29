import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowOeUpdateComponent } from './workflow-oe-update.component';

describe('WorkflowOeUpdateComponent', () => {
  let component: WorkflowOeUpdateComponent;
  let fixture: ComponentFixture<WorkflowOeUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowOeUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowOeUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

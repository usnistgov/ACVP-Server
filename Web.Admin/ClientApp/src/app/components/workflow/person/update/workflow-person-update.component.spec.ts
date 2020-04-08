import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowPersonUpdateComponent } from './workflow-person-update.component';

describe('WorkflowPersonUpdateComponent', () => {
  let component: WorkflowPersonUpdateComponent;
  let fixture: ComponentFixture<WorkflowPersonUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowPersonUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowPersonUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

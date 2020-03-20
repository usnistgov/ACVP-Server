import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowPersonDeleteComponent } from './workflow-person-delete.component';

describe('WorkflowPersonDeleteComponent', () => {
  let component: WorkflowPersonDeleteComponent;
  let fixture: ComponentFixture<WorkflowPersonDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowPersonDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowPersonDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowPersonCreateComponent } from './workflow-person-create.component';

describe('WorkflowPersonCreateComponent', () => {
  let component: WorkflowPersonCreateComponent;
  let fixture: ComponentFixture<WorkflowPersonCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowPersonCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowPersonCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowProductCreateComponent } from './workflow-product-create.component';

describe('WorkflowProductCreateComponent', () => {
  let component: WorkflowProductCreateComponent;
  let fixture: ComponentFixture<WorkflowProductCreateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowProductCreateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowProductCreateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

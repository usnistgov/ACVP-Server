import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowProductDeleteComponent } from './workflow-product-delete.component';

describe('WorkflowProductDeleteComponent', () => {
  let component: WorkflowProductDeleteComponent;
  let fixture: ComponentFixture<WorkflowProductDeleteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowProductDeleteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowProductDeleteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WorkflowProductUpdateComponent } from './workflow-product-update.component';

describe('WorkflowProductUpdateComponent', () => {
  let component: WorkflowProductUpdateComponent;
  let fixture: ComponentFixture<WorkflowProductUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkflowProductUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkflowProductUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

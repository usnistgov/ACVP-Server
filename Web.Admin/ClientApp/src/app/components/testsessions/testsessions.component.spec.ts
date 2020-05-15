import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestsessionsComponent } from './testsessions.component';

describe('TestsessionsComponent', () => {
  let component: TestsessionsComponent;
  let fixture: ComponentFixture<TestsessionsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestsessionsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestsessionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

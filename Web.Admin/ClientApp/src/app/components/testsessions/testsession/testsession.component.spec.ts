import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestsessionComponent } from './testsession.component';

describe('TestsessionComponent', () => {
  let component: TestsessionComponent;
  let fixture: ComponentFixture<TestsessionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestsessionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestsessionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

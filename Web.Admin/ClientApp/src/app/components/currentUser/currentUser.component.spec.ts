import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CurrentUserComponent } from './currentUser.component';

describe('CurrentUserComponent', () => {
  let component: CurrentUserComponent;
  let fixture: ComponentFixture<CurrentUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CurrentUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CurrentUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

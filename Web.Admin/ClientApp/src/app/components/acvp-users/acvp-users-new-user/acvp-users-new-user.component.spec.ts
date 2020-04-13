import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcvpUsersNewUserComponent } from './acvp-users-new-user.component';

describe('AcvpUsersNewUserComponent', () => {
  let component: AcvpUsersNewUserComponent;
  let fixture: ComponentFixture<AcvpUsersNewUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcvpUsersNewUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcvpUsersNewUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

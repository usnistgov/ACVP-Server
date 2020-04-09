import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcvpUsersComponent } from './acvp-users.component';

describe('AcvpUsersComponent', () => {
  let component: AcvpUsersComponent;
  let fixture: ComponentFixture<AcvpUsersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcvpUsersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcvpUsersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

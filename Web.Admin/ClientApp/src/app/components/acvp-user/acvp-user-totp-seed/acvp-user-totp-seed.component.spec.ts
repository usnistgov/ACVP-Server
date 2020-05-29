import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcvpUserTotpSeedComponent } from './acvp-user-totp-seed.component';

describe('AcvpUserTotpSeedComponent', () => {
  let component: AcvpUserTotpSeedComponent;
  let fixture: ComponentFixture<AcvpUserTotpSeedComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcvpUserTotpSeedComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcvpUserTotpSeedComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

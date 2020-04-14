import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcvpUserCertificateComponent } from './acvp-user-certificate.component';

describe('AcvpUserCertificateComponent', () => {
  let component: AcvpUserCertificateComponent;
  let fixture: ComponentFixture<AcvpUserCertificateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcvpUserCertificateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcvpUserCertificateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

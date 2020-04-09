import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcvpUserComponent } from './acvp-user.component';

describe('AcvpUserComponent', () => {
  let component: AcvpUserComponent;
  let fixture: ComponentFixture<AcvpUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AcvpUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AcvpUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

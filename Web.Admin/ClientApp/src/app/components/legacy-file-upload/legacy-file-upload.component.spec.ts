import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegacyFileUploadComponent } from './legacy-file-upload.component';

describe('LegacyFileUploadComponent', () => {
  let component: LegacyFileUploadComponent;
  let fixture: ComponentFixture<LegacyFileUploadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegacyFileUploadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegacyFileUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

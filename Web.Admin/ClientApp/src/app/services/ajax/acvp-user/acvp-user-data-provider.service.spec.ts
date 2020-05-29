import { TestBed } from '@angular/core/testing';

import { AcvpUserDataProviderService } from './acvp-user-data-provider.service';

describe('AcvpUserDataProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AcvpUserDataProviderService = TestBed.get(AcvpUserDataProviderService);
    expect(service).toBeTruthy();
  });
});

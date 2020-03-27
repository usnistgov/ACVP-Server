import { TestBed } from '@angular/core/testing';

import { OperatingEnvironmentProviderService } from './operating-environment-provider.service';

describe('OperatingEnvironmentProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OperatingEnvironmentProviderService = TestBed.get(OperatingEnvironmentProviderService);
    expect(service).toBeTruthy();
  });
});

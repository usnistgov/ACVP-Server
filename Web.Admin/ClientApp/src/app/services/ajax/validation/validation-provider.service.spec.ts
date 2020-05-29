import { TestBed } from '@angular/core/testing';

import { ValidationProviderService } from './validation-provider.service';

describe('ValidationProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ValidationProviderService = TestBed.get(ValidationProviderService);
    expect(service).toBeTruthy();
  });
});

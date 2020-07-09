import { TestBed } from '@angular/core/testing';

import { AddressProviderService } from './address-provider.service';

describe('AddressProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AddressProviderService = TestBed.get(AddressProviderService);
    expect(service).toBeTruthy();
  });
});

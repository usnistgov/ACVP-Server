import { TestBed } from '@angular/core/testing';

import { OrganizationProviderService } from './organization-provider.service';

describe('OrganizationProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: OrganizationProviderService = TestBed.get(OrganizationProviderService);
    expect(service).toBeTruthy();
  });
});

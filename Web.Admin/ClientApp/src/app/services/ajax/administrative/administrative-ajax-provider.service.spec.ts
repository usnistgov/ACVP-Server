import { TestBed } from '@angular/core/testing';

import { AdministrativeAjaxProviderService } from './administrative-ajax-provider.service';

describe('AdministrativeAjaxProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdministrativeAjaxProviderService = TestBed.get(AdministrativeAjaxProviderService);
    expect(service).toBeTruthy();
  });
});

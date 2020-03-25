import { TestBed } from '@angular/core/testing';

import { DependencyDataProviderService } from './dependency-data-provider.service';

describe('DependencyDataProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: DependencyDataProviderService = TestBed.get(DependencyDataProviderService);
    expect(service).toBeTruthy();
  });
});

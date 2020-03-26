import { TestBed } from '@angular/core/testing';

import { PersonProviderService } from './person-provider.service';

describe('PersonProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PersonProviderService = TestBed.get(PersonProviderService);
    expect(service).toBeTruthy();
  });
});

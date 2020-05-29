import { TestBed } from '@angular/core/testing';

import { TestSessionProviderService } from './test-session-provider.service';

describe('TestSessionProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: TestSessionProviderService = TestBed.get(TestSessionProviderService);
    expect(service).toBeTruthy();
  });
});

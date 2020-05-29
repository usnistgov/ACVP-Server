import { TestBed } from '@angular/core/testing';

import { WorkflowProviderService } from './workflow-provider.service';

describe('WorkflowProviderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: WorkflowProviderService = TestBed.get(WorkflowProviderService);
    expect(service).toBeTruthy();
  });
});

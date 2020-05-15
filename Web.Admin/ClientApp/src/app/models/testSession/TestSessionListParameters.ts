import { PagedEnumerable } from '../responses/PagedEnumerable';
import { TestSessionStatus } from './TestSessionStatus';

export class TestSessionListParameters extends PagedEnumerable {
  TestSessionId: string;
  VectorSetId: string;
  TestSessionStatus: string;

  constructor(TestSessionId: string, VectorSetId: string, TestSessionStatus: string) {
    super();
    this.TestSessionId = TestSessionId;
    this.VectorSetId = VectorSetId;
    this.TestSessionStatus = TestSessionStatus;
  }
}

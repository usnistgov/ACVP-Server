import { TestSessionStatus } from './TestSessionStatus';

export class TestSessionLite {

  testSessionId: number;
  created: Date;
  status: TestSessionStatus;

  public TestSessionLite(testSessionId: number,
    created: Date,
    status: TestSessionStatus) {
    this.testSessionId = testSessionId;
    this.created = created;
    this.status = status;
  };

}

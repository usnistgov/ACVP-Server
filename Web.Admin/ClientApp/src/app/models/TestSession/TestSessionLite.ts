import { TestSessionStatus } from './TestSessionStatus';

export class TestSessionLite {

  id: number;
  created: Date;
  status: TestSessionStatus;

  public TestSessionLite(id: number,
    created: Date,
    status: TestSessionStatus) {
    this.id = id;
    this.created = created;
    this.status = status;
  };

}

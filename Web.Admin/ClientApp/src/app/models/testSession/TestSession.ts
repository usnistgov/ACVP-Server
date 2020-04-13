import { TestSessionStatus } from './TestSessionStatus';
import { VectorSet } from './VectorSet';

export class TestSession {

  testSessionId: number;
  created: Date;
  userID: number;
  userName: string;
  status: TestSessionStatus;
  passedOn: Date;
  publishable: boolean;
  published: boolean;
  isSample: boolean;
  vectorSets: VectorSet[];

}

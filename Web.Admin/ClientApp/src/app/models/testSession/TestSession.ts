import { TestSessionStatus } from './TestSessionStatus';
import { VectorSet } from './VectorSet';

export class TestSession {

  testSessionId: number;
  created: Date;
  userID: number;
  userName: string;
  status: TestSessionStatus;
  isSample: boolean;
  vectorSets: VectorSet[];

}

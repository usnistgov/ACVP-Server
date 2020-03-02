import { TestSessionStatus } from './TestSessionStatus';
import { VectorSet } from './VectorSet';

export class TestSession {

  id: number;
  created: Date;
  status: TestSessionStatus;
  passedOn: Date;
  publishable: boolean;
  published: boolean;
  isSample: boolean;
  vectorSets: VectorSet[];

}

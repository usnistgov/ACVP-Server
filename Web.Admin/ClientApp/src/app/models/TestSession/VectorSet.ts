import { VectorSetStatus } from './VectorSetStatus';

export class VectorSet {

  id: number;
  generatorVersion: string;
  algorithmId: number
  algorithm: string;
  status: VectorSetStatus;
  jsonFilesAvailable: string[];

}

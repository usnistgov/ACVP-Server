import { VectorSetStatus } from './VectorSetStatus';
import { VectorSetResetOption } from './VectorSetResetOption.enum';

export class VectorSet {

  id: number;
  generatorVersion: string;
  algorithmId: number
  algorithm: string;
  status: VectorSetStatus;
  jsonFilesAvailable: string[];
  resetOption: VectorSetResetOption
}

import { WorkflowItemBase } from '../WorkflowItemBase';
import { Dependency } from '../../dependency/dependency';

export class OperatingEnvironmentCreatePayload extends WorkflowItemBase {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string;
  dependenciesToCreate: Dependency[];
}

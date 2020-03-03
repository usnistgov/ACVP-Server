import { WorkflowItemBase } from '../WorkflowItemBase';

export class WorkflowDependencyCreatePayload extends WorkflowItemBase {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string
  cpe: string;
}

import { WorkflowItemBase } from '../WorkflowItemBase';

export class WorkflowDependencyCreatePayload {
  id: number;
  url: string;
  type: string;
  name: string;
  description: string
  cpe: string;
}

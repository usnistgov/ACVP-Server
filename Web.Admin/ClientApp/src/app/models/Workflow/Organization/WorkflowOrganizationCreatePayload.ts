import { WorkflowItemBase } from '../WorkflowItemBase';
import { Address } from '../../Address/Address';

export class WorkflowOrganizationCreatePayload {
  id: number;
  url: string;
  name: string;
  link: string;
  parentUrl: string;
  emails: string[];
  voiceNumber: string;
  faxNumber: string;
  addresses: Address[];
}
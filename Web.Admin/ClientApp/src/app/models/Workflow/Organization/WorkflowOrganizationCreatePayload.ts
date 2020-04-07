import { Address } from '../../address/Address';

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

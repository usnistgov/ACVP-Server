import { Organization } from '../../organization/Organization';
import { WorkflowCreateProductPayloadContact } from './WorkflowCreateProductPayloadContact';
import { Address } from '../../address/Address';

export class WorkflowProductCreatePayload {
  id: number;
  url: string;
  name: string;
  description: string;
  type: string;
  version: string;
  website: string;
  address: Address;
  addressUrl: string;
  vendor: Organization;
  vendorUrl: string;
  contacts: WorkflowCreateProductPayloadContact[];
  contactUrls: string[];
  
}

import { Organization } from '../../Organization/Organization';
import { WorkflowCreateProductPayloadContact } from './WorkflowCreateProductPayloadContact';
import { Address } from '../../Address/Address';

export class WorkflowProductCreatePayload {
  id: number;
  url: string;
  name: string;
  description: string;
  type: string;
  version: string;
  link: string;
  addressUrl: null;
  address: Address;
  vendor: Organization;
  contacts: WorkflowCreateProductPayloadContact[];
  
}

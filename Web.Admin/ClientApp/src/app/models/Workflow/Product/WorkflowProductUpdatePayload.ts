import { Organization } from '../../Organization/Organization';
import { WorkflowCreateProductPayloadContact } from './WorkflowCreateProductPayloadContact';
import { Address } from '../../Address/Address';

export class WorkflowProductUpdatePayload {
  id: number;
  website: string;
  name: string;
  description: string;
  type: string;
  version: string;
  link: string;
  addressUrl: null;
  address: Address;
  vendor: Organization;
  contacts: WorkflowCreateProductPayloadContact[];

  nameUpdated: boolean;
  descriptionUpdated: boolean;
  typeUpdated: boolean;
  versionUpdated: boolean;
  websiteUpdated: boolean;
  vendorURLUpdated: boolean;
  addressURLUpdated: boolean;
  contactURLsUpdated: boolean;
  
}
import { Organization } from '../../organization/Organization';
import { WorkflowCreateProductPayloadContact } from './WorkflowCreateProductPayloadContact';
import { Address } from '../../address/Address';

export class WorkflowProductUpdatePayload {
  id: number;
  website: string;
  name: string;
  description: string;
  type: string;
  version: string;
  link: string;
  addressUrl: string;
  address: Address;
  vendorUrl: string;
  vendor: Organization;
  contactUrls: string[];
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

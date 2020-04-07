import { Address } from '../../Address/Address';

export class WorkflowOrganizationUpdatePayload {
  id: number;
  url: string;
  name: string;
  link: string;
  parentUrl: string;
  emails: string[];
  voiceNumber: string;
  faxNumber: string;
  addresses: Address[];

  nameUpdated: boolean;
  websiteUpdated: boolean;
  parentOrganizationURLUpdated: boolean;
  voiceNumberUpdate: boolean;
  faxNumberUpdated: boolean;
  emailAddressesUpdated: boolean;
  addressesUpdated: boolean;
}

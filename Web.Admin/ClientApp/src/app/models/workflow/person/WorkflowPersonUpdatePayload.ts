import { PersonPhone } from '../../person/PersonPhone';
import { Organization } from '../../organization/Organization';

export class WorkflowPersonUpdatePayload {
  id: number;
  url: string;
  fullName: string;
  vendorUrl: string;
  emails: string[];
  phoneNumbers: PersonPhone[];

  nameUpdated: boolean;
  organizationURLUpdated: boolean;
  phoneNumbersUpdated: boolean;
  emailAddressesUpdated: boolean;

  // This is only used as a place to store the value of the vendor-to-be's fields in an udpate
  organization: Organization;
}

import { PersonPhone } from '../../Person/PersonPhone';
import { Organization } from '../../Organization/Organization';

export class WorkflowPersonUpdatePayload {
  id: number;
  url: string;
  fullName: string;
  organizationUrl: string;
  emails: string[];
  phoneNumbers: PersonPhone[];

  nameUpdated: boolean;
  organizationURLUpdated: boolean;
  phoneNumbersUpdated: boolean;
  emailAddressesUpdated: boolean;

  // This is only used as a place to store the value of the vendor-to-be's fields in an udpate
  organization: Organization;
}

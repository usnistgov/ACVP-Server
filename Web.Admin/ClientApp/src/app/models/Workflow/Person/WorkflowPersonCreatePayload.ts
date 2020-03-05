import { PersonPhone } from '../../Person/PersonPhone';

export class WorkflowPersonCreatePayload {
  id: number;
  url: string;
  fullName: string;
  vendorUrl: string;
  emails: string[];
  phoneNumbers: PersonPhone[];
}

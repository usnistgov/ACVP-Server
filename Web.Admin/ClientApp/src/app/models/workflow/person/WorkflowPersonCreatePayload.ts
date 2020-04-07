import { PersonPhone } from '../../Person/PersonPhone';

export class WorkflowPersonCreatePayload {
  id: number;
  url: string;
  fullName: string;
  organizationUrl: string;
  emails: string[];
  phoneNumbers: PersonPhone[];
}

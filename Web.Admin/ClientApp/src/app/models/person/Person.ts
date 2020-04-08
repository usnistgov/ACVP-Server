import { Organization } from '../organization/Organization';
import { PersonPhone } from './PersonPhone';
import { PersonNote } from './PersonNote';

export class Person {

  id: number;
  name: string;
  organization: Organization;
  phoneNumbers: PersonPhone[];
  emailAddresses: string[];
  notes: PersonNote[];

  public OperatingEnvironment(id: number,
    name: string,
    organization: Organization,
    phoneNumbers: PersonPhone[],
    emailAddresses: string[],
    notes: PersonNote[]) {
    this.id = id;
    this.name = name;
    this.organization = organization;
    this.phoneNumbers = phoneNumbers;
    this.emailAddresses = emailAddresses;
    this.notes = notes;
  };

}

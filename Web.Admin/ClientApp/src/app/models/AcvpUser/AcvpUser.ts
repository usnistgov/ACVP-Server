import { Person } from '../person/Person';

export class AcvpUser {
  certificateBase64: string;
  commonName: string;
  seed: string;
  expiresOn: Date;
  issuer: string;
  acvpUserID: number;
  personID: number;
  person: Person;
  fullName: string;
  companyId: number;
  companyName: string;
}

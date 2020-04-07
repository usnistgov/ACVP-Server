import { Address } from '../Address/Address';
import { PersonLite } from '../Person/PersonLite';

export class Organization {

  id: number;
  name: string;
  url: string;
  addresses: Address[];
  voiceNumber: string;
  faxNumber: string;
  parent: Organization;
  persons: PersonLite[];
  emails: string[];

  public constructor(id: number,
    name: string,
    url: string,
    addresses: Address[],
    voiceNumber: string,
    faxNumber: string,
    parent: Organization) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.addresses = addresses;
    this.voiceNumber = voiceNumber;
    this.faxNumber = faxNumber;
    this.parent = parent;
  };

}

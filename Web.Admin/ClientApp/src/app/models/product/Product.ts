import { Organization } from '../organization/Organization';
import { Address } from '../address/Address';
import { Person } from '../person/Person';

export class Product {

  id: number;
  vendor: Organization;
  name: string;
  url: string;
  type: string;
  link: string;
  version: string;
  description: string;
  itar: boolean;
  address: Address;
  contacts: Person[];

  public Product(id: number,
    name: string,
    vendor: Organization,
    url: string,
    version: string,
    description: string,
    itar: boolean,
    address: Address,
    contacts: Person[]) {
    this.id = id;
    this.name = name;
    this.vendor = vendor;
    this.url = url;
    this.version = version;
    this.description = description;
    this.itar = itar;
    this.address = address;
    this.contacts = contacts;
  };

}

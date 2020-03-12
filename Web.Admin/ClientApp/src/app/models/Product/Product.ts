import { Organization } from '../Organization/Organization';
import { Address } from '../Address/Address';

export class Product {

  id: number;
  vendor: Organization;
  name: string;
  url: string;
  type: string;
  version: string;
  description: string;
  itar: boolean;
  address: Address;

  public Product(id: number,
    name: string,
    vendor: Organization,
    url: string,
    version: string,
    description: string,
    itar: boolean,
    address: Address) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.version = version;
    this.description = description;
    this.itar = itar;
    this.address = address;
  };

}

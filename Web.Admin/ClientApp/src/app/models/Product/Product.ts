import { Organization } from '../Organization/Organization';

export class Product {

  id: number;
  vendor: Organization;
  name: string;
  url: string;
  version: string;
  description: string;
  itar: boolean;

  public OperatingEnvironment(id: number,
    name: string,
    vendor: Organization,
    url: string,
    version: string,
    description: string,
    itar: boolean) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.version = version;
    this.description = description;
    this.itar = itar;
  };

}

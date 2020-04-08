export class Address {

  id: number;
  street1: string;
  street2: string;
  street3: string;
  locality: string;
  region: string;
  country: string;
  postalCode: string;

  public constructor(id: number,
    street1: string,
    street2: string,
    street3: string,
    locality: string,
    region: string,
    country: string,
    postalCode: string) {
    this.id = id;
    this.street1 = street1;
    this.street2 = street2;
    this.street3 = street3;
    this.locality = locality;
    this.region = region;
    this.country = country;
    this.postalCode = postalCode;
  };

}

export class AddressCreateParameters {

  organizationID: number;
  orderIndex: number;
  street1: string;
  street2: string;
  street3: string;
  locality: string;
  region: string;
  country: string;
  postalCode: string;

  public constructor(organizationID: number,
    orderIndex: number,
    street1: string,
    street2: string,
    street3: string,
    locality: string,
    region: string,
    country: string,
    postalCode: string) {
    this.organizationID = organizationID;
    this.orderIndex = orderIndex;
    this.street1 = street1;
    this.street2 = street2;
    this.street3 = street3;
    this.locality = locality;
    this.region = region;
    this.country = country;
    this.postalCode = postalCode;
  };

}

export class Organization {

  id: number;
  name: string;
  url: string;
  voiceNumber: string;
  faxNumber: string;
  parent: Organization;

  public constructor(id: number,
    name: string,
    url: string,
    voiceNumber: string,
    faxNumber: string,
    parent: Organization) {
    this.id = id;
    this.name = name;
    this.url = url;
    this.voiceNumber = voiceNumber;
    this.faxNumber = faxNumber;
    this.parent = parent;
  };

}

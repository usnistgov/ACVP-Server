import { PagedEnumerable } from '../responses/PagedEnumerable';

export class PersonListParameters extends PagedEnumerable {

  name: string;
  id: string;
  organizationName: string;

  public constructor(name: string, id: string, organizationName: string) {
    super();
    this.name = "";
    this.id = "";
    this.organizationName = "";
  }
}

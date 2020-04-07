import { PagedEnumerable } from '../responses/PagedEnumerable';

export class OrganizationListParameters extends PagedEnumerable {

  id: string;
  name: string;

  public constructor(id: string, name: string) {
    super();
    this.id = id;
    this.name = name;
  }
}

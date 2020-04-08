import { PagedEnumerable } from '../responses/PagedEnumerable';

export class ProductListParameters extends PagedEnumerable {

  name: string;
  id: string;
  description: string;

  public constructor(name: string, id: string, description: string) {
    super();
    this.name = name;
    this.id = id;
    this.description = description;
  }
}

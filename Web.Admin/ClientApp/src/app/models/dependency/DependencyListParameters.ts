import { PagedEnumerable } from '../responses/PagedEnumerable';

export class DependencyListParameters extends PagedEnumerable {

  name: string;
  type: string;
  description: string;

  public constructor(name: string, type: string, description: string) {
    super();
    this.name = "";
    this.type = "";
    this.description = "";
  }
}

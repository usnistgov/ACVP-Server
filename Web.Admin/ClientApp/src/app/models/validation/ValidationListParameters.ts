import { PagedEnumerable } from '../responses/PagedEnumerable';

export class ValidationListParameters extends PagedEnumerable {

  validationId: string;
  validationLabel: string;
  implementationName: string;

  public constructor(validationId: string, validationLabel: string, implementationName: string) {
    super();
    this.validationId = validationId;
    this.validationLabel = validationLabel;
    this.implementationName = implementationName;
  }
}

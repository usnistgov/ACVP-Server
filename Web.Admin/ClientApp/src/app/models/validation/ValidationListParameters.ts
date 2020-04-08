import { PagedEnumerable } from '../responses/PagedEnumerable';

export class ValidationListParameters extends PagedEnumerable {

  validationId: string;
  validationLabel: string;
  productName: string;

  public constructor(validationId: string, validationLabel: string, productName: string) {
    super();
    this.validationId = validationId;
    this.validationLabel = validationLabel;
    this.productName = productName;
  }
}

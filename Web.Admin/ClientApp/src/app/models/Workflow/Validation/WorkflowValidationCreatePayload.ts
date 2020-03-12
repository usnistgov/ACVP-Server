import { OperatingEnvironment } from '../../operatingEnvironment/operatingEnvironment';
import { Product } from '../../Product/Product';
import { WorkflowValidationCreatePayloadCertification } from './WorkflowValidationCreatePayloadCertification';

export class WorkflowValidationCreatePayload {
  testSessionId: number;

  productUrl: string;
  product: Product;

  oeUrl: string;
  oe: OperatingEnvironment;

  certification: WorkflowValidationCreatePayloadCertification;
}

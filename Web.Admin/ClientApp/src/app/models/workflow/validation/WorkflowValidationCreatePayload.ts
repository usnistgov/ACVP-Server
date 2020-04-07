import { OperatingEnvironment } from '../../operatingEnvironment/OperatingEnvironment';
import { Product } from '../../product/Product';
import { WorkflowValidationCreatePayloadCertification } from './WorkflowValidationCreatePayloadCertification';

export class WorkflowValidationCreatePayload {
  testSessionId: number;

  productUrl: string;
  product: Product;

  oeUrl: string;
  oe: OperatingEnvironment;

  certification: WorkflowValidationCreatePayloadCertification;
}

import { OperatingEnvironment } from '../../operatingEnvironment/OperatingEnvironment';
import { Product } from '../../product/Product';
import { WorkflowValidationCreatePayloadCertification } from './WorkflowValidationCreatePayloadCertification';
import { TestSession } from '../../testSession/TestSession';

export class WorkflowValidationCreatePayload {
  testSessionId: number;
  testSession: TestSession; // Used only for storing data during two-way data-binding in Angular

  productUrl: string;
  product: Product;

  oeUrl: string;
  oe: OperatingEnvironment;

  certification: WorkflowValidationCreatePayloadCertification;
}

import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { Validation } from './Validation';

export class ValidationList implements IWrappedEnumerable {
  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: Validation[];
}

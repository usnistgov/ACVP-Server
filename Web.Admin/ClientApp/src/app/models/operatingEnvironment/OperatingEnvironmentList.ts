import { OperatingEnvironment } from './OperatingEnvironment';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class OperatingEnvironmentList implements IWrappedEnumerable {
  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: OperatingEnvironment[];
}

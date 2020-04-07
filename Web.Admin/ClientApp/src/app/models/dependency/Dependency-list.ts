import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { DependencyLite } from './Dependency-lite';

export class DependencyList implements IWrappedEnumerable {
  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: DependencyLite[];
}

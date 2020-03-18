import { Product } from './Product';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class ProductList implements IWrappedEnumerable {
  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: Product[];
}

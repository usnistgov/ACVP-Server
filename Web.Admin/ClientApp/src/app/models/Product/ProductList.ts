import { Product } from './Product';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class ProductList implements IWrappedEnumerable {
  data: Product[];
}

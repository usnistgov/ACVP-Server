import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { DependencyLite } from './dependency-lite';

export class DependencyList implements IWrappedEnumerable {
  data: DependencyLite[];
}

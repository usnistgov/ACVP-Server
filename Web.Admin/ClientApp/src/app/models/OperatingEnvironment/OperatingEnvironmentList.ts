import { OperatingEnvironment } from './OperatingEnvironment';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class OperatingEnvironmentList implements IWrappedEnumerable {
  data: OperatingEnvironment[];
}

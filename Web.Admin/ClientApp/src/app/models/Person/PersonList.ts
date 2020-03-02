import { Person } from './Person';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class PersonList implements IWrappedEnumerable {
  data: Person[];
}

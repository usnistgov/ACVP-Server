import { Person } from './Person';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class PersonList implements IWrappedEnumerable {
  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: Person[];
}

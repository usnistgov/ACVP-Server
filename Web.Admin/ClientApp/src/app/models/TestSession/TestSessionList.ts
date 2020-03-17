import { TestSessionLite } from './TestSessionLite';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class TestSessionList implements IWrappedEnumerable {

  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;
  data: TestSessionLite[];

}

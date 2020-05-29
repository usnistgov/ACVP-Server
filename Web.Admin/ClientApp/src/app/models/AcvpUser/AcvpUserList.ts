import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { AcvpUserLite } from './AcvpUserLite';

export class AcvpUserList implements IWrappedEnumerable {

  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: AcvpUserLite[];
}

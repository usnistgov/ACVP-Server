import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';
import { OrganizationLite } from './OrganizationLite';

export class OrganizationList implements IWrappedEnumerable {

  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: OrganizationLite[];
}

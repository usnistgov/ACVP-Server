import { OrganizationLite } from './OrganizationLite';
import { IWrappedEnumerable } from '../../interfaces/wrapped-enumerable';

export class OrganizationList implements IWrappedEnumerable {

  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;

  data: OrganizationLite[];
}

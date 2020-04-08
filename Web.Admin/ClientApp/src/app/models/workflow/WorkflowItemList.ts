import { WorkflowItemLite } from './WorkflowItemLite';

export class WorkflowItemList {

  pageSize: number;
  currentPage: number;
  totalRecords: number;
  totalPages: number;
  data: WorkflowItemLite[];

}

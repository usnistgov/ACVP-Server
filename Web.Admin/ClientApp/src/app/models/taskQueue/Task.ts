export class Task {
  id: number;
  type: string;
  taskTypeText: string;
  vectorSetID: number;
  isSample: boolean;
  showExpected: boolean;
  status: string;
  createdOn: Date;

  // Used only for linking from the TaskQueue page to the relevant TS listing
  testSessionId: number;
}

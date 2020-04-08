export enum WorkflowStatus {
  Pending = 'Pending',
  Incomplete = 'Incomplete',
  Approved = 'Approved',
  Rejected = 'Rejected'
}

// This portion is sued to enable us to iterate over the values in the HTML to create the workflow
// page's APIAction dropdown select box
export namespace WorkflowStatus {

  export function values() {
    return Object.keys(WorkflowStatus).filter(
      (type) => isNaN(<any>type) && type !== 'values'
    );
  }
}

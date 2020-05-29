export enum TestSessionStatus {
  All = -1,
  Unknown = 0,
  Cancelled = 1,
  PendingEvaluation = 2,
  Failed = 3,
  Passed = 4,
  SubmittedForApproval = 5,
  Published = 6
}

// This portion is sued to enable us to iterate over the values in the HTML to create the TestSession
// page's status dropdown select box
export namespace TestSessionStatus {

  export function values() {
    return Object.keys(TestSessionStatus).filter(
      (type) => isNaN(<any>type) && type !== 'values'
    );
  }

}

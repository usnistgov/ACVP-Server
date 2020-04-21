export class LegacyResult {

  success: boolean;
  errors: string;
  submissionLogId: number;
  submissionId: string;
  validationNumber: number;
  labName: string;
  labPOC: string;
  labPOCEmail: string;

  public LegacyResult(
    success: boolean,
    errors: string,
    submissionLogId: number,
    submissionId: string,
    validationNumber: number,
    labName: string,
    labPOC: string,
    labPOCEmail: string) {
    this.success = success;
    this.errors = errors;
    this.submissionLogId = submissionLogId;
    this.submissionId = submissionId;
    this.validationNumber = validationNumber;
    this.labName = labName;
    this.labPOC = labPOC;
    this.labPOCEmail = labPOCEmail;
  }

}

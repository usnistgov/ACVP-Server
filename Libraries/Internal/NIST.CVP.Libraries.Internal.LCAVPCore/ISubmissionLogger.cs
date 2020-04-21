using System;

namespace NIST.CVP.Libraries.Internal.LCAVPCore
{
	public interface ISubmissionLogger
	{
		int LogSubmission(string submissionID, string labName, string labPOC, string labPOCEmail, string submissionType, DateTime receivedDate, int status, string zipFileName, string extractedFileLocation, string errorJson);
		bool LogSubmissionRegistration(int submissionID, int status, WorkflowType? workflowType, string registrationJson, string errorJson);
		void UpdateValidationIDForSubmissionLogID(int submissionLogID, long validationID);
	}
}
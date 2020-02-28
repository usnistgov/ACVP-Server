using System;

namespace LCAVPCore
{
	public interface ISubmissionLogger
	{
		int LogSubmission(string submissionID, string labName, string labPOC, string labPOCEmail, string submissionType, DateTime receivedDate, int status, string senderEmailAddress, string zipFileName, string extractedFileLocation, string errorJson);
		bool LogSubmissionRegistration(int submissionID, int status, WorkflowType? workflowType, string registrationJson, string errorJson);
	}
}
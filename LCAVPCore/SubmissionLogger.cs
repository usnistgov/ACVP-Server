using System;
using CVP.DatabaseInterface;
using Mighty;

namespace LCAVPCore
{
	public class SubmissionLogger : ISubmissionLogger
	{
		private readonly string _acvpConnectionString;

		public SubmissionLogger(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public int LogSubmission(string submissionID, string labName, string labPOC, string labPOCEmail, string submissionType, DateTime receivedDate, int status, string zipFileName, string extractedFileLocation, string errorJson)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.SingleFromProcedure("[lcavp].[SubmissionLogInsert]", inParams: new
				{
					SubmissionID = submissionID,
					LabName = labName,
					LabPOC = labPOC,
					LabPOCEmail = labPOCEmail,
					SubmissionType = submissionType,
					ReceivedDate = receivedDate,
					Status = status,
					SenderEmailAddress = "",
					ZipFileName = zipFileName,
					ExtractedFileLocation = extractedFileLocation,
					ErrorJson = errorJson
				});

				return (int)data.SubmissionLogID;
			}
			catch
			{
				return -1;
			}
		}

		public bool LogSubmissionRegistration(int submissionID, int status, WorkflowType? workflowType, string registrationJson, string errorJson)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				db.ExecuteProcedure("[lcavp].[SubmissionRegistrationInsert]", inParams: new
				{
					SubmissionID = submissionID,
					Status = status,
					WorkflowTypeID = workflowType,
					RegistrationJson = registrationJson,
					ErrorJson = errorJson
				});

				return true;
			}
			catch
			{
				return false;
			}
		}

		public void UpdateValidationIDForSubmissionLogID(int submissionLogID, long validationID)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				db.ExecuteProcedure("lcavp.UpdateValidationIDForSubmissionLogID", inParams: new
				{
					SubmissionLogID = submissionLogID,
					ValidationId = validationID
				});
			}
			catch
			{
				//Meh
			}
		}
	}
}

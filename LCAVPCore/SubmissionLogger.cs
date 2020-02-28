using System;
using System.Linq;
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

		public int LogSubmission(string submissionID, string labName, string labPOC, string labPOCEmail, string submissionType, DateTime receivedDate, int status, string senderEmailAddress, string zipFileName, string extractedFileLocation, string errorJson)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.Query("EXEC [lcavp].[SubmissionLogInsert] @0, @1, @2, @3, @4, @5, @6, @7, @8, @9, @10", submissionID, labName, labPOC, labPOCEmail, submissionType, receivedDate, status, senderEmailAddress, zipFileName, extractedFileLocation, errorJson).FirstOrDefault();

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

				db.Execute("EXEC [lcavp].[SubmissionRegistrationInsert] @0, @1, @2, @3, @4", submissionID, status, workflowType, registrationJson, errorJson);

				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}

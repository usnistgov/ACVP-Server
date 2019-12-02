using ACVPWorkflow.Results;
using CVP.DatabaseInterface;
using Mighty;
using System;

namespace ACVPWorkflow.Providers
{
	public class WorkflowProvider : IWorkflowProvider
	{
		private string _acvpConnectionString;

		public WorkflowProvider(IConnectionStringFactory connectionStringFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
		}

		public WorkflowInsertResult Insert(WorkflowItemType workflowItemType, RequestAction action, string labName, string contact, string email, string json)
		{
			try
			{
				var db = new MightyOrm(_acvpConnectionString);

				var data = db.ExecuteProcedure("val.WorkflowInsert", inParams: new
				{
					WorkflowItemType = workflowItemType,
					Action = action,
					Status = WorkflowStatus.Pending,
					LabName = labName,
					LabContactName = contact,
					LabContactEmail = email,
					Json = json
				}).FirstOrDefault();

				if (data != null)
				{
					return new WorkflowInsertResult { WorkflowID = data.WorflowID };
				}
				else return new WorkflowInsertResult { ErrorMessage = "Workflow item creation failed" };
			}
			catch (Exception ex)
			{
				return new WorkflowInsertResult { ErrorMessage = ex.Message };
			}
		}
	}
}

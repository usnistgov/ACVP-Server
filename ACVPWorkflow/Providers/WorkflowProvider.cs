using ACVPWorkflow.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using System;

namespace ACVPWorkflow.Providers
{
	public class WorkflowProvider : IWorkflowProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<WorkflowProvider> _logger;

		public WorkflowProvider(IConnectionStringFactory connectionStringFactory, ILogger<WorkflowProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public WorkflowInsertResult Insert(APIAction apiAction, WorkflowItemType workflowItemType, RequestAction action, long userID, string json, string labName, string contact, string email)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("val.WorkflowInsert", inParams: new
				{
					APIActionID = apiAction,
					WorkflowItemType = workflowItemType,
					Action = action,
					Status = WorkflowStatus.Pending,
					LabName = labName,
					LabContactName = contact,
					LabContactEmail = email,
					Json = json,
					RequestingUserId = userID
				});

				if (data != null)
				{
					return new WorkflowInsertResult { WorkflowID = (long)data.WorkflowID };
				}
				else return new WorkflowInsertResult("Workflow item creation failed");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new WorkflowInsertResult(ex.Message);
			}
		}

		public Result Update(long workflowItemID, WorkflowStatus status, long acceptID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.WorkflowUpdateStatusAcceptID", inParams: new
				{
					WorkflowItemID = workflowItemID,
					Status = status,
					AcceptID = acceptID
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public Result Update(long workflowItemID, WorkflowStatus status)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.ExecuteProcedure("val.WorkflowUpdateStatus", inParams: new
				{
					WorkflowItemID = workflowItemID,
					Status = status
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}
	}
}

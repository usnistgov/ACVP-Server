using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.Enumerables;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models.Parameters;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Results;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.ACVPWorkflow.Providers
{
	public class WorkflowProvider : IWorkflowProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<WorkflowProvider> _logger;
		private readonly IWorkflowItemPayloadFactory _workflowItemPayloadFactory;

		public WorkflowProvider(IConnectionStringFactory connectionStringFactory, ILogger<WorkflowProvider> logger, IWorkflowItemPayloadFactory workflowItemPayloadFactory)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
			_workflowItemPayloadFactory = workflowItemPayloadFactory;
		}

		public WorkflowInsertResult Insert(APIAction apiAction, long userID, string json, string labName, string contact, string email)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.WorkflowInsert", inParams: new
				{
					APIActionID = apiAction,
					WorkflowStatusId = WorkflowStatus.Pending,
					LabName = labName,
					LabContactName = contact,
					LabContactEmail = email,
					RequestingUserId = userID,
					Json = json
				});

				if (data != null)
				{
					return new WorkflowInsertResult { WorkflowID = (long)data.WorkflowItemId };
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
				var data = db.ExecuteProcedure("dbo.WorkflowUpdateStatusAcceptID", inParams: new
				{
					WorkflowItemId = workflowItemID,
					WorkflowStatusId = status,
					AcceptId = acceptID
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
				var data = db.ExecuteProcedure("dbo.WorkflowUpdateStatus", inParams: new
				{
					WorkflowItemId = workflowItemID,
					@WorkflowStatusId = status
				});

				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param)
		{
			var result = new List<WorkflowItemLite>();
			long totalRecords = 0;
			var db = new MightyOrm<WorkflowItemLite>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("dbo.WorkflowItemsGet", inParams:
					new
					{
						PageSize = param.PageSize,
						Page = param.Page,
						WorkflowItemId = param.WorkflowItemId,
						APIActionID = param.APIActionId,
						RequestId = param.RequestId,
						WorkflowStatusId = param.Status
					}, outParams:
					new
					{
						totalRecords = (long) 0
					});

				result = dbResult.Data;
				totalRecords = dbResult.ResultsExpando.totalRecords;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
			}

			return result.ToPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public WorkflowItem GetWorkflowItem(long workflowItemId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.WorkflowItemGetById", new
				{
					workflowItemId
				});

				if (data == null)
					return null;
				
				return new WorkflowItem()
				{
					RequestId = data.RequestId,
					APIAction = (APIAction)data.APIActionID,
					Payload = _workflowItemPayloadFactory.GetPayload(data.JsonBlob, (APIAction)data.APIActionId),
					WorkflowItemID = workflowItemId,
					Status = (WorkflowStatus)data.WorkflowStatusId,
					AcceptId = data.AcceptId
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return null;
			}
		}
	}
}

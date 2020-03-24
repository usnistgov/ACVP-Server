using System;
using System.Collections.Generic;
using ACVPWorkflow.Models;
using ACVPWorkflow.Models.Parameters;
using ACVPWorkflow.Results;
using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Enumerables;
using NIST.CVP.ExtensionMethods;
using NIST.CVP.Results;

namespace ACVPWorkflow.Providers
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

		public PagedEnumerable<WorkflowItemLite> GetWorkflowItems(WorkflowListParameters param)
		{
			var result = new List<WorkflowItemLite>();
			long totalRecords = 0;
			var db = new MightyOrm<WorkflowItemLite>(_acvpConnectionString);

			try
			{
				var dbResult = db.QueryWithExpando("acvp.WorkflowItemsGet",
					new
					{
						param.PageSize,
						param.Page,
						param.WorkflowItemId,
						param.Type,
						param.RequestId
					},
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

			return result.WrapPagedEnumerable(param.PageSize, param.Page, totalRecords);
		}

		public WorkflowItem GetWorkflowItem(long workflowItemId)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("acvp.WorkflowItemGetById", new
				{
					workflowItemId
				});

				if (data == null)
					return null;
				
				return new WorkflowItem()
				{
					APIAction = (APIAction)data.APIActionId,
					Payload = _workflowItemPayloadFactory.GetPayload(data.JsonBlob, (APIAction)data.APIActionId),
					WorkflowItemID = workflowItemId,
					Status = (WorkflowStatus)data.Status
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

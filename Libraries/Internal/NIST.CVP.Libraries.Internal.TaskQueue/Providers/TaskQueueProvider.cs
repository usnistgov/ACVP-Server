using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.Libraries.Shared.ExtensionMethods;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.TaskQueue.Providers
{
	public class TaskQueueProvider : ITaskQueueProvider
	{
		private readonly string _acvpConnectionString;
		private readonly ILogger<TaskQueueProvider> _logger;

		public TaskQueueProvider(IConnectionStringFactory connectionStringFactory, ILogger<TaskQueueProvider> logger)
		{
			_acvpConnectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
			_logger = logger;
		}

		public Result Insert(TaskType type, long vectorSetID, bool isSample, bool showExpected)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TaskQueueInsert", inParams: new
				{
					TaskType = type,
					VectorSetId = vectorSetID,
					IsSample = isSample,
					ShowExpected = showExpected
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}

		public List<TaskQueueItem> List()
		{
			var db = new MightyOrm<TaskQueueItem>(_acvpConnectionString);

			return db.QueryFromProcedure("dbo.TaskQueueList").ToList();
		}

		public TaskQueueItem GetNext()
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				var data = db.SingleFromProcedure("dbo.TaskQueueGet");

				return data == null ? null : new TaskQueueItem
				{
					ID = data.TaskId,
					Type = (TaskType)data.TaskTypeId,
					VectorSetID = data.VectorSetID,
					IsSample = data.IsSample,
					ShowExpected = data.ShowExpected,
					Status = data.Status,
					CreatedOn = data.CreatedOn
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return null;
			}
		}

		public Result Delete(long taskID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TaskQueueDelete", inParams: new { TaskID = taskID });
				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public Result DeletePendingTasksForVectorSet(long vectorSetID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TaskQueueDeletePendingForVectorSet", inParams: new { VectorSetId = vectorSetID });
				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public Result RestartAll()
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TaskQueueRestart");
				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}
		}

		public Result UpdateStatus(long taskID, TaskStatus taskStatus)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("dbo.TaskQueueSetStatus", new
				{
					TaskID = taskID,
					Status = taskStatus
				});
				return new Result();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex);
				return new Result(ex.Message);
			}
		}
	}
}

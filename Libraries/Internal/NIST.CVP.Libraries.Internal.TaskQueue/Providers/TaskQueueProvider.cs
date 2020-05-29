using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Mighty;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
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
			//Map the type in the task queue record with something easier to work with
			string taskType = type switch
			{
				TaskType.Generation => "vector-generation",
				TaskType.Validation => "vector-validation",
				_ => null
			};

			//Error if not a valid task type
			if (taskType == null)
			{
				return new Result("Invalid task type");
			}

			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("common.TaskQueueInsert", inParams: new
				{
					TaskType = taskType,
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

			return db.QueryFromProcedure("common.TaskQueueList").ToList();
		}

		public Result Delete(long taskID)
		{
			var db = new MightyOrm(_acvpConnectionString);

			try
			{
				db.ExecuteProcedure("common.TaskQueueDelete", inParams: new { TaskID = taskID });
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
				db.ExecuteProcedure("common.TaskQueueDeletePendingForVectorSet", inParams: new { VectorSetId = vectorSetID });
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
				db.ExecuteProcedure("common.TaskQueueRestart");
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

using System;
using NIST.CVP.Results;

using CVP.DatabaseInterface;
using Microsoft.Extensions.Logging;
using Mighty;

namespace ACVPCore.Providers
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

		public Result Insert(TaskType type, long vectorSetID, bool isSample)
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
					IsSample = isSample
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.Message);
				return new Result(ex.Message);
			}

			return new Result();
		}
	}
}

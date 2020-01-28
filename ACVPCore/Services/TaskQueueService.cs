using ACVPCore.Providers;
using ACVPCore.Results;
using System.Text.Json;

namespace ACVPCore.Services
{
	public class TaskQueueService : ITaskQueueService
	{
		private readonly ITaskQueueProvider _taskQueueProvider;

		public TaskQueueService (ITaskQueueProvider taskQueueProvider)
		{
			_taskQueueProvider = taskQueueProvider;
		}

		public Result AddGenerationTask(GenerationTask task)
		{
			return _taskQueueProvider.Insert(TaskType.Generation, JsonSerializer.Serialize(task));
		}

		public Result AddValidationTask(ValidationTask task)
		{
			//Validation task payload is just the vector set ID as a string. A little odd that it isn't JSON like everything else
			return _taskQueueProvider.Insert(TaskType.Validation, task.VectorSetID.ToString());
		}
	}
}

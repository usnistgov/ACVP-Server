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
			return _taskQueueProvider.Insert(TaskType.Generation, task.VectorSetID, task.IsSample);
		}

		public Result AddValidationTask(ValidationTask task)
		{
			return _taskQueueProvider.Insert(TaskType.Validation, task.VectorSetID, false);
		}
	}
}

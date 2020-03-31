using System.Collections.Generic;
using NIST.CVP.Results;
using NIST.CVP.TaskQueue.Providers;

namespace NIST.CVP.TaskQueue.Services
{
	public class TaskQueueService : ITaskQueueService
	{
		private readonly ITaskQueueProvider _taskQueueProvider;

		public TaskQueueService (ITaskQueueProvider taskQueueProvider)
		{
			_taskQueueProvider = taskQueueProvider;
		}

		public Result AddGenerationTask(GenerationTask task) => _taskQueueProvider.Insert(TaskType.Generation, task.VectorSetID, task.IsSample);

		public Result AddValidationTask(ValidationTask task) => _taskQueueProvider.Insert(TaskType.Validation, task.VectorSetID, false);

		public Result Delete(long taskID) => _taskQueueProvider.Delete(taskID);

		public List<TaskQueueItem> List() => _taskQueueProvider.List();

		public Result RestartAll() => _taskQueueProvider.RestartAll();
	}
}

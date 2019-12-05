using ACVPCore.Providers;
using System.Text.Json;

namespace ACVPCore.Services
{
	public class TaskQueueService : ITaskQueueService
	{
		private ITaskQueueProvider _taskQueueProvider;

		public TaskQueueService (ITaskQueueProvider taskQueueProvider)
		{
			_taskQueueProvider = taskQueueProvider;
		}

		public void AddGenerationTask(GenerationTask task)
		{
			_taskQueueProvider.Insert(TaskType.Generation, JsonSerializer.Serialize(task));
		}
	}
}

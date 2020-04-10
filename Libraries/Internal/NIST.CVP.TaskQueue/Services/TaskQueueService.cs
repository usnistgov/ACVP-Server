using System.Collections.Generic;
using ACVPCore;
using ACVPCore.Services;
using NIST.CVP.Results;
using NIST.CVP.TaskQueue.Providers;

namespace NIST.CVP.TaskQueue.Services
{
	public class TaskQueueService : ITaskQueueService
	{
		private readonly ITaskQueueProvider _taskQueueProvider;
		private readonly ITestSessionService _testSessionService;
		private readonly IVectorSetService _vectorSetService;

		public TaskQueueService (ITaskQueueProvider taskQueueProvider, ITestSessionService testSessionService, IVectorSetService vectorSetService)
		{
			_taskQueueProvider = taskQueueProvider;
			_testSessionService = testSessionService;
			_vectorSetService = vectorSetService;
		}

		public Result AddGenerationTask(GenerationTask task) => _taskQueueProvider.Insert(TaskType.Generation, task.VectorSetID, task.IsSample);

		public Result AddValidationTask(ValidationTask task) => _taskQueueProvider.Insert(TaskType.Validation, task.VectorSetID, false);
		public Result RequeueGenerationTask(long vectorSetId)
		{
			var testSessionId = _testSessionService.GetTestSessionIDFromVectorSet(vectorSetId);
			var testSession = _testSessionService.Get(testSessionId);
			
			var task = new GenerationTask()
			{
				IsSample = testSession.IsSample,
				VectorSetID = vectorSetId
			};
			
			
			_vectorSetService.UpdateStatus(task.VectorSetID, VectorSetStatus.Initial);
			_vectorSetService.RemoveVectorFileJson(task.VectorSetID, VectorSetJsonFileTypes.Error);
			_testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(task.VectorSetID);
			
			return AddGenerationTask(task);
		}

		public Result RequeueValidationTask(long vectorSetId)
		{
			var task = new ValidationTask()
			{
				VectorSetID = vectorSetId
			};
			
			_vectorSetService.UpdateStatus(task.VectorSetID, VectorSetStatus.Processed);
			_vectorSetService.RemoveVectorFileJson(task.VectorSetID, VectorSetJsonFileTypes.Error);
			_testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(task.VectorSetID);

			return AddValidationTask(task);
		}

		public Result Delete(long taskID) => _taskQueueProvider.Delete(taskID);

		public List<TaskQueueItem> List() => _taskQueueProvider.List();

		public Result RestartAll() => _taskQueueProvider.RestartAll();
	}
}

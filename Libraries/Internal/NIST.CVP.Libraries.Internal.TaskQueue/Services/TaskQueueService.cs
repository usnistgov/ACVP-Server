using System.Collections.Generic;
using System.Text.Json;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Internal.TaskQueue.Providers;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.MessageQueue.Abstractions.Models;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.TaskQueue.Services
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

		public Result AddGenerationTask(GenerationTask task) => _taskQueueProvider.Insert(TaskType.Generation, task.VectorSetID, task.IsSample, false);

		public Result AddValidationTask(ValidationTask task) => _taskQueueProvider.Insert(TaskType.Validation, task.VectorSetID, false, task.ShowExpected);
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
			var originalJson = _vectorSetService.GetVectorFileJson(vectorSetId, VectorSetJsonFileTypes.SubmittedAnswers);
			var projectedObject = JsonSerializer.Deserialize<VectorSetSubmissionPayload>(originalJson);
			
			var task = new ValidationTask()
			{
				VectorSetID = vectorSetId,
				ShowExpected = projectedObject.ShowExpected
			};
			
			_vectorSetService.UpdateStatus(task.VectorSetID, VectorSetStatus.Processed);
			_vectorSetService.RemoveVectorFileJson(task.VectorSetID, VectorSetJsonFileTypes.Error);
			_testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(task.VectorSetID);

			return AddValidationTask(task);
		}

		public Result Delete(long taskID) => _taskQueueProvider.Delete(taskID);

		public Result DeletePendingTasksForVectorSet(long vectorSetID) => _taskQueueProvider.DeletePendingTasksForVectorSet(vectorSetID);

		public List<TaskQueueItem> List() => _taskQueueProvider.List();

		public Result RestartAll() => _taskQueueProvider.RestartAll();

		public TaskQueueItem GetNext() => _taskQueueProvider.GetNext();

		public Result UpdateStatus(long taskID, TaskStatus taskStatus) => _taskQueueProvider.UpdateStatus(taskID, taskStatus);
	}
}

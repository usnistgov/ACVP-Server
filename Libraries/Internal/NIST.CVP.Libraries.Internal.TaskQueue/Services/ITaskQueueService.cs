using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.TaskQueue.Services
{
	public interface ITaskQueueService
	{
		Result AddGenerationTask(GenerationTask task);
		Result AddValidationTask(ValidationTask task);
		Result RequeueGenerationTask(long vectorSetid);
		Result RequeueValidationTask(long vectorSetId);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result DeletePendingTasksForVectorSet(long vectorSetID);
		Result RestartAll();
		TaskQueueItem GetNext();
		Result UpdateStatus(long taskID, TaskStatus taskStatus);
	}
}
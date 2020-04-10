using System.Collections.Generic;
using NIST.CVP.Results;


namespace NIST.CVP.TaskQueue.Services
{
	public interface ITaskQueueService
	{
		Result AddGenerationTask(GenerationTask task);
		Result AddValidationTask(ValidationTask task);
		Result RequeueGenerationTask(long vectorSetid);
		Result RequeueValidationTask(long vectorSetId);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result RestartAll();
	}
}
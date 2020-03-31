using System.Collections.Generic;
using NIST.CVP.Results;


namespace NIST.CVP.TaskQueue.Services
{
	public interface ITaskQueueService
	{
		Result AddGenerationTask(GenerationTask task);
		Result AddValidationTask(ValidationTask task);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result RestartAll();
	}
}
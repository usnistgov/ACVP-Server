using System.Collections.Generic;
using NIST.CVP.Results;


namespace NIST.CVP.TaskQueue.Providers
{
	public interface ITaskQueueProvider
	{
		Result Insert(TaskType type, long vectorSetID, bool isSample);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result RestartAll();
	}
}
using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;


namespace NIST.CVP.Libraries.Internal.TaskQueue.Providers
{
	public interface ITaskQueueProvider
	{
		Result Insert(TaskType type, long vectorSetID, bool isSample);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result RestartAll();
	}
}
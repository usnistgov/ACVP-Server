using System.Collections.Generic;
using NIST.CVP.Libraries.Shared.Results;

namespace NIST.CVP.Libraries.Internal.TaskQueue.Providers
{
	public interface ITaskQueueProvider
	{
		Result Insert(TaskType type, long vectorSetID, bool isSample, bool showExpected);
		List<TaskQueueItem> List();
		Result Delete(long taskID);
		Result DeletePendingTasksForVectorSet(long vectorSetID);
		Result RestartAll();
		TaskQueueItem GetNext();
		Result UpdateStatus(long taskID, TaskStatus taskStatus);
	}
}
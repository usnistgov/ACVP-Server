using NIST.CVP.Libraries.Internal.TaskQueue;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface ITaskProvider
    {
        ExecutableTask GetTaskFromQueue();
        void SetTaskStatus(long taskID, TaskStatus status);
    }
}
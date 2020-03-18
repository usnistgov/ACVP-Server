using System.Data;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface ITaskProvider
    {
        ExecutableTask GetTaskFromQueue();
    }
}
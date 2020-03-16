using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface ITaskService
    {
        Task<long> RunTask(ExecutableTask task);
        ExecutableTask GetTaskFromQueue();
    }
}
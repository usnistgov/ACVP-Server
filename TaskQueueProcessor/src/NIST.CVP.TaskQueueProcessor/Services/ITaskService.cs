using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface ITaskService
    {
        void RunTask(ExecutableTask task);
        Task RunTaskAsync(ExecutableTask task);
        ExecutableTask GetTaskFromQueue();
    }
}
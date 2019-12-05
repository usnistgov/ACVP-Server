using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public interface ITaskRunner
    {
        Task<int> RunTask(ITask task);
    }
}
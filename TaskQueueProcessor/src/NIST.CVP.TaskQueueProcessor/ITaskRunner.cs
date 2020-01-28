using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public interface ITaskRunner
    {
        Task<long> RunTask(ITask task);
    }
}
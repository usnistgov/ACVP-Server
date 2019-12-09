using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public class TaskRunner : ITaskRunner
    {
        public async Task<int> RunTask(ITask task)
        {
            task.Run();
            return await Task.FromResult(task.DbId);
        }
    }
}
using System.Threading.Tasks;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor
{
    public class TaskRunner : ITaskRunner
    {
        public async Task<long> RunTask(ITask task)
        {
            Log.Information($"Running job: {task.DbId}");
            await task.Run(); // stop executing method until you have a result from the task
            return task.DbId;
        }
    }
}
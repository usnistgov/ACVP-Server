using System;
using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public class TaskRunner : ITaskRunner
    {
        public async Task<long> RunTask(ITask task)
        {
            task.Run();
            Console.WriteLine($"Running job: {task.DbId}");
            return await Task.FromResult(task.DbId);
        }
    }
}
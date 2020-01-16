using System;
using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public class TaskRunner : ITaskRunner
    {
        public async Task<long> RunTask(ITask task)
        {
            Console.WriteLine($"Running job: {task.DbId}");
            await task.Run(); // stop executing method until you have a result from the task
            return task.DbId;
        }
    }
}
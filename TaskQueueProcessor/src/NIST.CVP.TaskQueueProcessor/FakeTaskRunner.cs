using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public class FakeTaskRunner
    {
        public async Task<int> RunTask(ITask task)
        {
            task.Run();
            return await Task.FromResult(task.DbId);
        }
    }
}
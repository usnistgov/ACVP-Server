using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IDbProvider
    {
        ITask GetNextTask();
        void DeleteCompletedTask(long taskId);
        void MarkTasksForRestart();
        void PutJson(int vsId, string jsonFileType, string jsonContent);
    }
}
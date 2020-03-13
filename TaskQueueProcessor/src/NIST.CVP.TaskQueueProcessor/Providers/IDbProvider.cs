using NIST.CVP.TaskQueueProcessor.Constants;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IDbProvider
    {
        ITask GetNextTask();
        void DeleteCompletedTask(long taskId);
        void MarkTasksForRestart();
        void PutJson(int vsId, string jsonFileType, string jsonContent);
        void SetStatus(int vsId, StatusType status, string errorMessage);
    }
}
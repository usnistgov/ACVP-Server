using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IDbProvider
    {
        ITask GetNextTask();
        void DeleteCompletedTask(long taskId);
        void MarkTasksForRestart();
        void PutJson(long vsId, string jsonFileType, string jsonContent);

        //void PutPromptData(GenerationTask task);
        //void PutValidationData(ValidationTask task);
        //void PutErrorData(ExecutableTask task);
    }
}
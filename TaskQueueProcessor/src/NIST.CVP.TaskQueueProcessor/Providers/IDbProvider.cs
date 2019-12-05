namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IDbProvider
    {
        ITask GetNextTask();
        void DeleteCompletedTask(long taskId);
        void MarkTasksForRestart();
    }
}
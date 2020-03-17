namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface ICleaningService
    {
        void DeleteCompletedTask(long taskId);
        void MarkTasksForRestart();
    }
}
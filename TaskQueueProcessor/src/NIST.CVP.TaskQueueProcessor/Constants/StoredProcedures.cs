namespace NIST.CVP.TaskQueueProcessor.Constants
{
    public static class StoredProcedures
    {
        public const string GET_TASK_QUEUE = "common.TaskQueueGet";
        public const string DELETE_TASK_FROM_TASK_QUEUE = "common.TaskQueueDelete";
        public const string UPDATE_IN_PROGRESS_TASK_TO_READY = "common.TaskQueueRestart";
    }
}
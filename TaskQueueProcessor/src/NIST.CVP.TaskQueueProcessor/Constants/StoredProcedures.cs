namespace NIST.CVP.TaskQueueProcessor.Constants
{
    public static class StoredProcedures
    {
        public const string GET_TASK_QUEUE = "common.TaskQueueGet";
        public const string DELETE_TASK_FROM_TASK_QUEUE = "common.TaskQueueDelete";
        public const string UPDATE_IN_PROGRESS_TASK_TO_READY = "common.TaskQueueRestart";
        public const string GET_JSON = "acvp.VectorSetJsonGet";
        public const string PUT_JSON = "acvp.VectorSetJsonPut";
        public const string SET_STATUS = "acvp.VectorSetUpdateStatusAndMessage";
    }
}
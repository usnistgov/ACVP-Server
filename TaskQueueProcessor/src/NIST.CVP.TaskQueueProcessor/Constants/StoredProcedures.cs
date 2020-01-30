namespace NIST.CVP.TaskQueueProcessor.Constants
{
    public static class StoredProcedures
    {
        public const string GET_TASK_QUEUE = "common.TaskQueueGet";
        public const string DELETE_TASK_FROM_TASK_QUEUE = "common.TaskQueueDelete";
        public const string UPDATE_IN_PROGRESS_TASK_TO_READY = "common.TaskQueueRestart";
        public const string GET_JSON = "acvp.VectorSetJsonGet";
        public const string PUT_JSON = "acvp.VectorSetJsonPut";
        
        public const string GET_CAPABILITIES = "acvp.CapabilitiesGet";
        public const string GET_SUBMITTED = "acvp.SubmittedGet";
        public const string PUT_ALL_PROMPT_DATA = "acvp.PromptDataPut";
        public const string PUT_VALIDATION = "acvp.ValidationPut";
        public const string PUT_ERROR = "acvp.ErrorPut";
    }
}
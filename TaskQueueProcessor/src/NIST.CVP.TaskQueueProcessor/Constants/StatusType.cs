namespace NIST.CVP.TaskQueueProcessor.Constants
{
    public enum StatusType
    {
        INITIAL = 0,
        PROCESSED = 1,
        KAT_RECEIVED = 2,
        PASSED = 3,
        FAILED = 4,
        CANCELLED = 5,
        ERROR = -1
    }
}
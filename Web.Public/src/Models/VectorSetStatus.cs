namespace Web.Public.Models
{
    public enum VectorSetStatus
    {
        Initial = 0,
        Processed = 1,
        KATReceived = 2,
        Passed = 3,
        Failed = 4,
        Cancelled = 5,
        Error = -1
    }
}
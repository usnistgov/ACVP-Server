namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class ValidationTask : ExecutableTask
    {
        public string SubmittedResults { get; set; }
        public string InternalProjection { get; set; }
        public bool Expected { get; set; }
        public string Validation { get; set; }
    }
}
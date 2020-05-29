namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask
    {
        public long DbId { get; set; }
        public long VsId { get; set; }
        
        public string Error { get; set; }
    }
}
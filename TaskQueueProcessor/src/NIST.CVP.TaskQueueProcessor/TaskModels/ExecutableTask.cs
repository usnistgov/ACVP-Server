namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask : ITask
    {
        public int DbId { get; set; }
        public int VsId { get; set; }
        public bool IsSample { get; set; }
        public abstract string ExecutablePath { get; set; }
        public int VectorSetExpectedResultsId { get; set; }
        
        public abstract void Run();
    }
}
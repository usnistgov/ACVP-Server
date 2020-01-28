namespace NIST.CVP.Common.Config
{
    public class TaskQueueProcessorConfig
    {
        public int PollDelay { get; set; }
        public int MaxConcurrency { get; set; }
    }
}
namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public bool IsSample { get; set; }    // This is in the capabilities already
        public string Capabilities { get; set; }
        public string Prompt { get; set; }
        public string InternalProjection { get; set; }
        public string ExpectedResults { get; set; }
    }
}
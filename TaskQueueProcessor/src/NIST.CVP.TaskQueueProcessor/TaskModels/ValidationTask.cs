using System;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class ValidationTask : ExecutableTask
    {
        public override string ExecutablePath { get; set; }
        public string SubmittedResults { get; set; }
        public string InternalProjection { get; set; }

        public override void Run()
        {
            Console.WriteLine($"Validation Task: {VsId}, {IsSample}");
        }
    }
}
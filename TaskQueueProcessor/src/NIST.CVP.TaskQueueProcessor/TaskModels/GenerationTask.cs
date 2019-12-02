using System;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public override string ExecutablePath { get; set; }
        public string Capabilities { get; set; }

        public override void Run()
        {
            // Call Gen/Vals with
            Console.WriteLine($"Generation Task: {VsId}, {IsSample}");
            Console.WriteLine($"Capabilities: {Capabilities}");
        }
    }
}
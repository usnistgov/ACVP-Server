using System;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public bool IsSample { get; set; }    // This is in the capabilities already
        public string Capabilities { get; set; }
        public string Prompt { get; set; }
        public string InternalProjection { get; set; }
        
        public GenerationTask(IGenValInvoker genValInvoker) : base(genValInvoker) { }
        
        public override void Run()
        {
            Console.WriteLine($"Generation Task VsId: {VsId}, IsSample: {IsSample}");
            Console.WriteLine($"Capabilities: {Capabilities}");
            
            var genRequest = new GenerateRequest(Capabilities);
            var response = GenValInvoker.Generate(genRequest);

            if (response.Success)
            {
                Console.WriteLine($"Success on vsId: {VsId}");
                Prompt = response.PromptProjection;
                InternalProjection = response.InternalProjection;
            }
            else
            {
                Console.WriteLine($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
            }
        }
    }
}
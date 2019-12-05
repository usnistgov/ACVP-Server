using System;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public bool IsSample { get; set; }
        public string Capabilities { get; set; }
        
        public GenerationTask(IGenValInvoker genValInvoker) : base(genValInvoker) { }
        
        public override void Run()
        {
            Console.WriteLine($"Generation Task: {VsId}, {IsSample}");
            Console.WriteLine($"Capabilities: {Capabilities}");
            
            var genRequest = new GenerateRequest(Capabilities);
            //var response = _genValInvoker.Generate(genRequest);
            
            //Console.WriteLine(response.Success);            
        }
    }
}
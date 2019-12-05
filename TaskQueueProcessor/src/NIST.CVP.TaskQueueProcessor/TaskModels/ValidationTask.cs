using System;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class ValidationTask : ExecutableTask
    {
        public string SubmittedResults { get; set; }
        public string InternalProjection { get; set; }
        public bool Expected { get; set; }

        public ValidationTask(IGenValInvoker genValInvoker) : base(genValInvoker) { }

        public override void Run()
        {
            Console.WriteLine($"Validation Task: {VsId}");
            
            var validateRequest = new ValidateRequest(InternalProjection, SubmittedResults, false);
            //var response = _genValInvoker.Validate(validateRequest);
            
            //Console.WriteLine(response.Success);
        }
    }
}
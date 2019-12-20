using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class ValidationTask : ExecutableTask
    {
        public string SubmittedResults { get; set; }
        public string InternalProjection { get; set; }
        public bool Expected { get; set; }
        public string Validation { get; set; }
        
        public ValidationTask(IGenValInvoker genValInvoker) : base(genValInvoker) { }
        
        public override async Task<object> Run()
        {
            Console.WriteLine($"Validation Task: {VsId}");
            
            var valRequest = new ValidateRequest(InternalProjection, SubmittedResults, Expected);
            var response = await Task.Factory.StartNew(() => GenValInvoker.Validate(valRequest));
            
            if (response.Success)
            {
                Console.WriteLine($"Success on vsId: {VsId}");
                Validation = response.ValidationResult;
            }
            else
            {
                Console.WriteLine($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
            }

            return Task.FromResult(response);
        }
    }
}
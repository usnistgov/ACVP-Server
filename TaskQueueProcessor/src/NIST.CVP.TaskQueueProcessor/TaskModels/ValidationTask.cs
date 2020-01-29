using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class ValidationTask : ExecutableTask
    {
        public string SubmittedResults { get; set; }
        public string InternalProjection { get; set; }
        public bool Expected { get; set; }
        public string Validation { get; set; }
        
        public ValidationTask(IGenValInvoker genValInvoker, IDbProvider dbProvider) : base(genValInvoker, dbProvider) { }
        
        public override async Task<object> Run()
        {
            Console.WriteLine($"Validation Task: {VsId}");
            
            var valRequest = new ValidateRequest(InternalProjection, SubmittedResults, Expected);
            var response = await Task.Factory.StartNew(() => GenValInvoker.Validate(valRequest));
            
            if (response.Success)
            {
                Console.WriteLine($"Success on vsId: {VsId}");
                Validation = response.ValidationResult;
                DbProvider.PutJson(VsId, JsonFileTypes.VALIDATION, Validation);
            }
            else
            {
                Console.WriteLine($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
                DbProvider.PutJson(VsId, JsonFileTypes.ERROR, Error);
            }

            return Task.FromResult(response);
        }
    }
}
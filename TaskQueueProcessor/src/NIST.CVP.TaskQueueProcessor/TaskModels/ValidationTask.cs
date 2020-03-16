using System.Threading.Tasks;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.Providers;
using Serilog;

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
            Log.Information($"Validation Task: {VsId}");
            
            var valRequest = new ValidateRequest(InternalProjection, SubmittedResults, Expected);
            var response = await Task.Factory.StartNew(() => GenValInvoker.Validate(valRequest, VsId));
            
            if (response.StatusCode == StatusCode.Success)
            {
                Log.Information($"Success on vsId: {VsId}");
                Validation = response.ValidationResult;
                DbProvider.SetStatus(VsId, StatusType.PASSED, "");
                DbProvider.PutJson(VsId, JsonFileTypes.VALIDATION, Validation);
            }
            else if (response.StatusCode == StatusCode.ValidatorFail)
            {
                Log.Information($"Incorrect response on vsId: {VsId}");
                Validation = response.ValidationResult;
                DbProvider.SetStatus(VsId, StatusType.FAILED, "");
                DbProvider.PutJson(VsId, JsonFileTypes.VALIDATION, Validation);
            }
            else
            {
                Log.Error($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
                DbProvider.SetStatus(VsId, StatusType.ERROR, "");
                DbProvider.PutJson(VsId, JsonFileTypes.ERROR, Error);
            }

            return Task.FromResult(response);
        }
    }
}
using System;
using System.Threading.Tasks;
using ACVPCore.Services;
using CVP.DatabaseInterface;
using Mighty;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.Providers;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class GenValService : IGenValService
    {
        private readonly IGenValInvoker _genValInvoker;
        private readonly IJsonProvider _jsonProvider;
        private readonly string _connectionString;
        private readonly ITestSessionService _testSessionService;
        
        public GenValService(IGenValInvoker genValInvoker, IJsonProvider jsonProvider, ITestSessionService testSessionService, IConnectionStringFactory connectionStringFactory)
        {
            _genValInvoker = genValInvoker;
            _jsonProvider = jsonProvider;
            _testSessionService = testSessionService;
            _connectionString = connectionStringFactory.GetMightyConnectionString("ACVP");
        }
        
        public async Task RunGeneratorAsync(GenerationTask generationTask)
        {
            Log.Information($"Generation Task VsId: {generationTask.VsId}, IsSample: {generationTask.IsSample}");
            Log.Debug($"Capabilities: {generationTask.Capabilities}");
            
            var genRequest = new GenerateRequest(generationTask.Capabilities);
            var response = await _genValInvoker.GenerateAsync(genRequest, generationTask.VsId);

            if (response.Success)
            {
                Log.Information($"Success on vsId: {generationTask.VsId}");
                generationTask.Prompt = response.PromptProjection;
                generationTask.InternalProjection = response.InternalProjection;
                generationTask.ExpectedResults = response.ResultProjection;

                _jsonProvider.PutJson(generationTask.VsId, JsonFileTypes.PROMPT, generationTask.Prompt);
                _jsonProvider.PutJson(generationTask.VsId, JsonFileTypes.INTERNAL_PROJECTION, generationTask.InternalProjection);
                _jsonProvider.PutJson(generationTask.VsId, JsonFileTypes.EXPECTED_RESULTS, generationTask.ExpectedResults);
                SetStatus(generationTask.VsId, StatusType.PROCESSED, "");
            }
            else
            {
                Log.Error($"Error on vsId: {generationTask.VsId}");
                generationTask.Error = response.ErrorMessage;

                _jsonProvider.PutJson(generationTask.VsId, JsonFileTypes.ERROR, generationTask.Error);
                SetStatus(generationTask.VsId, StatusType.ERROR, "");
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(generationTask.VsId);
            }
        }

        public void RunValidator(ValidationTask validationTask)
        {
            Log.Information($"Validation Task: {validationTask.VsId}");
            
            var valRequest = new ValidateRequest(validationTask.InternalProjection, validationTask.SubmittedResults, validationTask.Expected);
            var response = _genValInvoker.Validate(valRequest, validationTask.VsId);
            
            if (response.StatusCode == StatusCode.Success)
            {
                Log.Information($"Success on vsId: {validationTask.VsId}");
                validationTask.Validation = response.ValidationResult;
                
                _jsonProvider.PutJson(validationTask.VsId, JsonFileTypes.VALIDATION, validationTask.Validation);
                SetStatus(validationTask.VsId, StatusType.PASSED, "");
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
            else if (response.StatusCode == StatusCode.ValidatorFail)
            {
                Log.Information($"Incorrect response on vsId: {validationTask.VsId}");
                validationTask.Validation = response.ValidationResult;
                
                _jsonProvider.PutJson(validationTask.VsId, JsonFileTypes.VALIDATION, validationTask.Validation);
                SetStatus(validationTask.VsId, StatusType.FAILED, "");
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
            else
            {
                Log.Error($"Error on vsId: {validationTask.VsId}");
                validationTask.Error = response.ErrorMessage;
                
                _jsonProvider.PutJson(validationTask.VsId, JsonFileTypes.ERROR, validationTask.Error);
                SetStatus(validationTask.VsId, StatusType.ERROR, "");
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
        }

        private void SetStatus(int vsId, StatusType status, string errorMessage)
        {
            var acvp = new MightyOrm(_connectionString);

            try
            {
                acvp.ExecuteProcedure(StoredProcedures.SET_STATUS, new
                {
                    VectorSetID = vsId,
                    Status = status,
                    ErrorMessage = errorMessage
                });
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Unable to set status: {status} for vsId: {vsId}");
            }
        }
    }
}
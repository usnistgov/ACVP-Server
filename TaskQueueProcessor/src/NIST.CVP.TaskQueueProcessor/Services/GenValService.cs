using System.Text.Json;
using System.Threading.Tasks;
using NIST.CVP.Common.Enums;
using NIST.CVP.Generation.Core;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using NIST.CVP.Libraries.Shared.ACVPCore.Abstractions;
using NIST.CVP.Libraries.Shared.DatabaseInterface;
using NIST.CVP.TaskQueueProcessor.TaskModels;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public class GenValService : IGenValService
    {
        private readonly IGenValInvoker _genValInvoker;
        private readonly IVectorSetService _vectorSetService;
        private readonly string _connectionString;
        private readonly ITestSessionService _testSessionService;
        
        public GenValService(IGenValInvoker genValInvoker, IVectorSetService vectorSetService, ITestSessionService testSessionService, IConnectionStringFactory connectionStringFactory)
        {
            _genValInvoker = genValInvoker;
            _vectorSetService = vectorSetService;
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

                //Populate VectorSetJson
                _vectorSetService.InsertVectorSetJson(generationTask.VsId, VectorSetJsonFileTypes.Prompt, generationTask.Prompt);
                _vectorSetService.InsertVectorSetJson(generationTask.VsId, VectorSetJsonFileTypes.InternalProjection, generationTask.InternalProjection);
                _vectorSetService.InsertVectorSetJson(generationTask.VsId, VectorSetJsonFileTypes.ExpectedAnswers, generationTask.ExpectedResults);

                //Update the vector set status
                _vectorSetService.UpdateStatus(generationTask.VsId, VectorSetStatus.Processed);
            }
            else
            {
                Log.Error($"Error on vsId: {generationTask.VsId}");
                generationTask.Error = response.ErrorMessage;

                // Turn the error into a JSON object
                var errorJson = JsonSerializer.Serialize(new
                {
                    error = response.ErrorMessage
                });

                //Populate the Error data in VectorSetJson
                _vectorSetService.InsertVectorSetJson(generationTask.VsId, VectorSetJsonFileTypes.Error, errorJson);

                //Update the vector set status
                _vectorSetService.UpdateStatus(generationTask.VsId, VectorSetStatus.Error);

                //Update the test session status
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(generationTask.VsId);
            }
        }

        public async Task RunValidatorAsync(ValidationTask validationTask)
        {
            Log.Information($"Validation Task: {validationTask.VsId}");
            
            var valRequest = new ValidateRequest(validationTask.InternalProjection, validationTask.SubmittedResults, validationTask.Expected);
            var response = await _genValInvoker.ValidateAsync(valRequest, validationTask.VsId);
            
            if (response.StatusCode == StatusCode.Success)
            {
                Log.Information($"Success on vsId: {validationTask.VsId}");
                validationTask.Validation = response.ValidationResult;

                //Populate the Validation data in VectorSetJson
                _vectorSetService.InsertVectorSetJson(validationTask.VsId, VectorSetJsonFileTypes.Validation, validationTask.Validation);

                //Update the vector set status
                _vectorSetService.UpdateStatus(validationTask.VsId, VectorSetStatus.Passed);

                //Update the test session status
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
            else if (response.StatusCode == StatusCode.ValidatorFail)
            {
                Log.Information($"Incorrect response on vsId: {validationTask.VsId}");
                validationTask.Validation = response.ValidationResult;
                
                //Populate the Validation data in VectorSetJson
                _vectorSetService.InsertVectorSetJson(validationTask.VsId, VectorSetJsonFileTypes.Validation, validationTask.Validation);

                //Update the vector set status
                _vectorSetService.UpdateStatus(validationTask.VsId, VectorSetStatus.Failed);

                //Update the test session status
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
            else
            {
                Log.Error($"Error on vsId: {validationTask.VsId}");
                validationTask.Error = response.ErrorMessage;
                
                // Turn the error into a JSON object
                var errorJson = JsonSerializer.Serialize(new
                {
                    error = response.ErrorMessage
                });

                //Populate the Error data in VectorSetJson
                _vectorSetService.InsertVectorSetJson(validationTask.VsId, VectorSetJsonFileTypes.Error, errorJson);

                //Update the vector set status
                _vectorSetService.UpdateStatus(validationTask.VsId, VectorSetStatus.Error);

                //Update the test session status
                _testSessionService.UpdateStatusFromVectorSetsWithVectorSetID(validationTask.VsId);
            }
        }
    }
}
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Constants;
using NIST.CVP.TaskQueueProcessor.Providers;
using Serilog;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public bool IsSample { get; set; }    // This is in the capabilities already
        public string Capabilities { get; set; }
        public string Prompt { get; set; }
        public string InternalProjection { get; set; }
        public string ExpectedResults { get; set; }
        
        public GenerationTask(IGenValInvoker genValInvoker, IDbProvider dbProvider) : base(genValInvoker, dbProvider) { }
        
        public override async Task<object> Run()
        {
            Log.Information($"Generation Task VsId: {VsId}, IsSample: {IsSample}");
            Log.Debug($"Capabilities: {Capabilities}");
            
            var genRequest = new GenerateRequest(Capabilities);
            var response = await Task.Factory.StartNew(() => GenValInvoker.Generate(genRequest, VsId));

            if (response.Success)
            {
                Log.Information($"Success on vsId: {VsId}");
                Prompt = response.PromptProjection;
                InternalProjection = response.InternalProjection;
                ExpectedResults = response.ResultProjection;

                DbProvider.PutJson(VsId, JsonFileTypes.PROMPT, Prompt);
                DbProvider.PutJson(VsId, JsonFileTypes.INTERNAL_PROJECTION, InternalProjection);
                DbProvider.PutJson(VsId, JsonFileTypes.EXPECTED_RESULTS, ExpectedResults);
            }
            else
            {
                Log.Error($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
                DbProvider.PutJson(VsId, JsonFileTypes.ERROR, Error);
            }

            return Task.FromResult(response);
        }
    }
}
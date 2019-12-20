using System;
using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class GenerationTask : ExecutableTask
    {
        public bool IsSample { get; set; }    // This is in the capabilities already
        public string Capabilities { get; set; }
        public string Prompt { get; set; }
        public string InternalProjection { get; set; }
        
        public GenerationTask(IGenValInvoker genValInvoker, IDbProvider dbProvider) : base(genValInvoker, dbProvider) { }
        
        public override async Task<object> Run()
        {
            Console.WriteLine($"Generation Task VsId: {VsId}, IsSample: {IsSample}");
            Console.WriteLine($"Capabilities: {Capabilities}");
            
            var genRequest = new GenerateRequest(Capabilities);
            var response = await Task.Factory.StartNew(() => GenValInvoker.Generate(genRequest));

            if (response.Success)
            {
                Console.WriteLine($"Success on vsId: {VsId}");
                Prompt = response.PromptProjection;
                InternalProjection = response.InternalProjection;
                DbProvider.PutPromptData(this);
            }
            else
            {
                Console.WriteLine($"Error on vsId: {VsId}");
                Error = response.ErrorMessage;
                DbProvider.PutErrorData(this);
            }

            return Task.FromResult(response);
        }
    }
}
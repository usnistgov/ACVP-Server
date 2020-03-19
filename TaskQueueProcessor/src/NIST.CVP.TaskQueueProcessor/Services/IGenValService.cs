using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface IGenValService
    {
        Task RunGeneratorAsync(GenerationTask generationTask);
        void RunValidator(ValidationTask validationTask);
    }
}
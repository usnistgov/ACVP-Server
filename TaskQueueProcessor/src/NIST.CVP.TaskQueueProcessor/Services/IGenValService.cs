using System.Threading.Tasks;
using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface IGenValService
    {
        Task<object> RunGenerator(GenerationTask generationTask);
        Task<object> RunValidator(ValidationTask validationTask);
    }
}
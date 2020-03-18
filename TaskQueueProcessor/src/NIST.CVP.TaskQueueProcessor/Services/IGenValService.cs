using NIST.CVP.TaskQueueProcessor.TaskModels;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface IGenValService
    {
        void RunGenerator(GenerationTask generationTask);
        void RunValidator(ValidationTask validationTask);
    }
}
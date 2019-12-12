using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask : ITask
    {
        public long DbId { get; set; }
        public int VsId { get; set; }
        
        public string Error { get; set; }
        public IGenValInvoker GenValInvoker { protected get; set; }
        
        public abstract void Run();
    }
}
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask : ITask
    {
        public long DbId { get; set; }
        public int VsId { get; set; }

        protected readonly IGenValInvoker _genValInvoker;

        protected ExecutableTask(IGenValInvoker genValInvoker)
        {
            _genValInvoker = genValInvoker;
        }
        
        public abstract void Run();
    }
}
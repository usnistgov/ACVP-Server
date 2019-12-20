using System.Threading.Tasks;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask : ITask
    {
        public long DbId { get; set; }
        public int VsId { get; set; }
        
        public string Error { get; set; }
        protected IGenValInvoker GenValInvoker { get; set; }

        protected ExecutableTask(IGenValInvoker genValInvoker)
        {
            GenValInvoker = genValInvoker;
        }
        
        public abstract Task<object> Run();
    }
}
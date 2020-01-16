using System.Threading.Tasks;
using NIST.CVP.Generation.Core;
using NIST.CVP.TaskQueueProcessor.Providers;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public abstract class ExecutableTask : ITask
    {
        public long DbId { get; set; }
        public int VsId { get; set; }
        
        public string Error { get; set; }
        protected IGenValInvoker GenValInvoker { get; }
        protected IDbProvider DbProvider { get; }

        protected ExecutableTask(IGenValInvoker genValInvoker, IDbProvider dbProvider)
        {
            GenValInvoker = genValInvoker;
            DbProvider = dbProvider;
        }
        
        public abstract Task<object> Run();
    }
}
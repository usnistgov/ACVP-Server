using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor.TaskModels
{
    public class NullTask
    {
        public long DbId { get; set; } = -1;
        public int VsId { get; set; } = -1;
        
        public Task<object> Run()
        {
            throw new System.NotImplementedException();
        }
    }
}
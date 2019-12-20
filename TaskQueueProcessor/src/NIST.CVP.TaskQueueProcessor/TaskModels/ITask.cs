using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor
{
    public interface ITask
    {
        long DbId { get; set; }
        int VsId { get; set; }

        Task<object> Run();
    }
}
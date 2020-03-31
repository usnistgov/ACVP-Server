using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor.Services
{
    public interface IPoolService
    {
        Task SpawnPoolDataAsync();
    }
}
using System.Threading.Tasks;

namespace NIST.CVP.TaskQueueProcessor.Providers
{
    public interface IPoolProvider
    {
        Task<object> SpawnPoolData();
    }
}
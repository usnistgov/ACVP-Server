using System.Threading.Tasks;
using Orleans;

namespace NIST.CVP.Crypto.Oracle
{
    public interface IClusterClientFactory
    {
        Task<IClusterClient> Get();
        Task ResetClient();
    }
}
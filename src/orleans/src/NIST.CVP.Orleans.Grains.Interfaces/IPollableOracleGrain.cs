using System.Threading.Tasks;
using NIST.CVP.Orleans.Grains.Interfaces.Enums;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IPollableOracleGrain<TResult>
    {
        Task<GrainState> CheckStatusAsync();
        Task<TResult> GetResultAsync();
    }
}
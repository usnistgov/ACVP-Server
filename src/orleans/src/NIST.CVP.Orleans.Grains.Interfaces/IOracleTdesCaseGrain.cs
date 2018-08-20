using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleTdesCaseGrain<TResult> :
        IPollableOracleGrain<TResult>, IGrainWithGuidKey
    {
        Task<bool> BeginWorkAsync(TdesParameters param);
    }
}
using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleAesCaseGrain<TResult> :
        IPollableOracleGrain<TResult>, IGrainWithGuidKey
    {
        Task<bool> BeginWorkAsync(AesParameters param);
    }
}
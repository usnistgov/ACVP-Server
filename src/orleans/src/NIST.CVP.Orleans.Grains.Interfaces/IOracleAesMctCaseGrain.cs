using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleAesMctCaseGrain : 
        IPollableOracleGrain<MctResult<AesResult>>, IGrainWithGuidKey
    {
        Task<bool> BeginWorkAsync(AesParameters param);
    }
}
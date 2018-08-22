using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleAesCcmCaseGrain : 
        IPollableOracleGrain<AeadResult>, IGrainWithGuidKey
    {
        Task<bool> BeginWorkAsync(AeadParameters param);
    }
}
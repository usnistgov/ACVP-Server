using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Dsa
{
    public interface IOracleObserverDsaPqVerifyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<DsaDomainParametersResult>>
    {
        Task<bool> BeginWorkAsync(DsaDomainParametersParameters param, DsaDomainParametersResult fullParam);
    }
}
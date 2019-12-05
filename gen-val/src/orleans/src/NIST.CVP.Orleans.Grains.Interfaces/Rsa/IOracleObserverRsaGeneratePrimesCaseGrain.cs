using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Rsa
{
    public interface IOracleObserverRsaGeneratePrimesCaseGrain : IGrainWithGuidKey, IGrainObservable<RsaPrimeResult>
    {
        Task<bool> BeginWorkAsync(RsaKeyParameters param);
    }
}
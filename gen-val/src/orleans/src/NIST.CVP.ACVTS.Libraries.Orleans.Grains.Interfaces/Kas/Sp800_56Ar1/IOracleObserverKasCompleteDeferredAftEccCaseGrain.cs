using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Ar1
{
    public interface IOracleObserverKasCompleteDeferredAftEccCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftDeferredResult>
    {
        Task<bool> BeginWorkAsync(KasAftDeferredParametersEcc param);
    }
}

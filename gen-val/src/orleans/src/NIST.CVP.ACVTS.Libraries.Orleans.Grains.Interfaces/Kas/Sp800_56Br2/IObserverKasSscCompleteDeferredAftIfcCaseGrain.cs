using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar3;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Br2;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.Sp800_56Br2
{
    public interface IObserverKasSscCompleteDeferredAftIfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasSscAftDeferredResultIfc>
    {
        Task<bool> BeginWorkAsync(KasSscAftDeferredParametersIfc param);
    }
}

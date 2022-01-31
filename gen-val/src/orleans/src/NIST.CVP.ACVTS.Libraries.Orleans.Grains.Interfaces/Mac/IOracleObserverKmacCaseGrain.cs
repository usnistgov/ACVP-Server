using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Mac
{
    public interface IOracleObserverKmacCaseGrain : IGrainWithGuidKey, IGrainObservable<KmacResult>
    {
        Task<bool> BeginWorkAsync(KmacParameters param);
    }
}

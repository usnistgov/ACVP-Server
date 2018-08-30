using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Mac
{
    public interface IOracleObserverKmacCaseGrain : IGrainWithGuidKey, IGrainObservable<KmacResult>
    {
        Task<bool> BeginWorkAsync(KmacParameters param);
    }
}
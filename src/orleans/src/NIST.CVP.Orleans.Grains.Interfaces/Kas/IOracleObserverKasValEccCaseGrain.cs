using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasValEccCaseGrain : IGrainWithGuidKey, IGrainObservable<KasValResultEcc>
    {
        Task<bool> BeginWorkAsync(KasValParametersEcc param);
    }
}
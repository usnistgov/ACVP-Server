using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.KeyConfirmation;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.KeyConfirmation;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kas.KeyConfirmation
{
    public interface IOracleKasKcAftCaseGrain : IGrainWithGuidKey, IGrainObservable<KasKcAftResult>
    {
        Task<bool> BeginWorkAsync(KasKcAftParameters param);
    }
}

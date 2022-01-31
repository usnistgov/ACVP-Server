using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Eddsa
{
    public interface IOracleObserverEddsaMessageBitFlipCaseGrain : IGrainWithGuidKey, IGrainObservable<BitString>
    {
        Task<bool> BeginWorkAsync(EddsaMessageParameters param);
    }
}

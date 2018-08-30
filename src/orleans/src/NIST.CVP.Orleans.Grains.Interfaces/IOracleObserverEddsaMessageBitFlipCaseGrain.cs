using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Math;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces
{
    public interface IOracleObserverEddsaMessageBitFlipCaseGrain : IGrainWithGuidKey, IGrainObservable<BitString>
    {
        Task<bool> BeginWorkAsync(EddsaMessageParameters param);
    }
}
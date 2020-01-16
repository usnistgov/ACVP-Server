using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.SafePrimes
{
    public interface IObserverSafePrimesKeyVerGrain : IGrainWithGuidKey, IGrainObservable<SafePrimesKeyVerResult>
    {
        Task<bool> BeginWorkAsync(SafePrimesKeyVerParameters param);
    }
}
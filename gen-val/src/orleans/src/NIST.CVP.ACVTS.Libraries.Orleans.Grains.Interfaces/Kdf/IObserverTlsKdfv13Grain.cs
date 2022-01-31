using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Kdf
{
    public interface IObserverTlsKdfv13Grain : IGrainWithGuidKey, IGrainObservable<TlsKdfv13Result>
    {
        Task<bool> BeginWorkAsync(TlsKdfv13Parameters param);
    }
}

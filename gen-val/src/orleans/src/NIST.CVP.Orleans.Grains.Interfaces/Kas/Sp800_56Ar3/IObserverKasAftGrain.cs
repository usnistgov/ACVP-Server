using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar3;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Ar3
{
    public interface IObserverKasAftGrain : IGrainWithGuidKey, IGrainObservable<KasAftResult>
    {
        Task<bool> BeginWorkAsync(KasAftParameters param);
    }
}
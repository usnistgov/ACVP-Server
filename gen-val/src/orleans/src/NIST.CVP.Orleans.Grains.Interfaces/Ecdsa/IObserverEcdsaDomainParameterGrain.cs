using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IObserverEcdsaDomainParameterGrain : IGrainWithGuidKey, IGrainObservable<EccDomainParameters>
    {
        Task<bool> BeginWorkAsync(Curve param);
    }
}
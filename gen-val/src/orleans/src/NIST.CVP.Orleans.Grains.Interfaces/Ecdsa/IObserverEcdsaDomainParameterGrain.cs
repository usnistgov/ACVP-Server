using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar3;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Ecdsa
{
    public interface IObserverEcdsaDomainParameterGrain : IGrainWithGuidKey, IGrainObservable<EccDomainParametersResult>
    {
        Task<bool> BeginWorkAsync(EcdsaCurveParameters param);
    }
}
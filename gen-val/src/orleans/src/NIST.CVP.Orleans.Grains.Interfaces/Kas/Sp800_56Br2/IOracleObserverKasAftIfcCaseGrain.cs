using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Br2;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Br2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas.Sp800_56Br2
{
    public interface IOracleObserverKasAftIfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftResultIfc>
    {
        Task<bool> BeginWorkAsync(KasAftParametersIfc param, KeyPair serverKeyPair, KeyPair iutKeyPair);
    }
}
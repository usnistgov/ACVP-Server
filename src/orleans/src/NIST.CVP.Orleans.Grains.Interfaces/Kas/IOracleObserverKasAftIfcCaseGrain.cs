using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kas
{
    public interface IOracleObserverKasAftIfcCaseGrain : IGrainWithGuidKey, IGrainObservable<KasAftResultIfc>
    {
        Task<bool> BeginWorkAsync(KasAftParametersIfc param, KeyPair serverKeyPair, KeyPair iutKeyPair);
    }
}
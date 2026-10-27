using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.XMSS.Native;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

public interface IOracleObserverXmssCompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<XmssVerificationResult>
{
    Task<bool> BeginWorkAsync(XmssSignatureParameters param, XmssSignatureResult providedResult);
}

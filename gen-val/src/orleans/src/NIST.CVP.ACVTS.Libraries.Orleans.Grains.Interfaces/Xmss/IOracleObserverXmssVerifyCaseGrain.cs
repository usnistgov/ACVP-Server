using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Xmss;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xmss;

public interface IOracleObserverXmssVerifyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<XmssSignatureResult>>
{
    Task<bool> BeginWorkAsync(XmssSignatureParameters param);
}

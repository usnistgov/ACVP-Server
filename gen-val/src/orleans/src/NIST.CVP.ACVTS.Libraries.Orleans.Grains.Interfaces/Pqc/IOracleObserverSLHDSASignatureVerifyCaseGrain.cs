using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.SLH_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SLH_DSA;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

public interface IOracleObserverSLHDSASignatureVerifyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<SLHDSASignatureResult>>
{
    Task<bool> BeginWorkAsync(SLHDSASignatureParameters param);
}

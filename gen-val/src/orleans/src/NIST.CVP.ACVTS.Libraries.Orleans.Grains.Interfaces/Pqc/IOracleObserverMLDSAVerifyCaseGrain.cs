using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

public interface IOracleObserverMLDSAVerifyCaseGrain : IGrainWithGuidKey, IGrainObservable<VerifyResult<MLDSASignatureResult>>
{
    Task<bool> BeginWorkAsync(MLDSASignatureParameters param);
}

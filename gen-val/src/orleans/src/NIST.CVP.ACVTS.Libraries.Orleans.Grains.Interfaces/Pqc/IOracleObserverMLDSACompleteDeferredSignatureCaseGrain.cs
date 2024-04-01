using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.PQC.Dilithium;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

public interface IOracleObserverMLDSACompleteDeferredSignatureCaseGrain : IGrainWithGuidKey, IGrainObservable<MLDSAVerificationResult>
{
    Task<bool> BeginWorkAsync(MLDSASignatureParameters param, MLDSASignatureResult providedResult);
}

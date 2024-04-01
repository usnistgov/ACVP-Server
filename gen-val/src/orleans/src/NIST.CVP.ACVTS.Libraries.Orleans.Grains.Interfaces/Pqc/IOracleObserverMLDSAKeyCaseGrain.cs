using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_DSA;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_DSA;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

public interface IOracleObserverMLDSAKeyCaseGrain : IGrainWithGuidKey, IGrainObservable<MLDSAKeyPairResult>
{
    Task<bool> BeginWorkAsync(MLDSAKeyGenParameters param);
}

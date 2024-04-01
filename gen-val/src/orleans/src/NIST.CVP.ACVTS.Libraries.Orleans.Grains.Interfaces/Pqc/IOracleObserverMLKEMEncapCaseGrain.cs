using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.ML_KEM;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.ML_KEM;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Pqc;

public interface IOracleObserverMLKEMEncapCaseGrain : IGrainWithGuidKey, IGrainObservable<MLKEMEncapsulationResult>
{
    Task<bool> BeginWorkAsync(MLKEMEncapsulationParameters param);
}

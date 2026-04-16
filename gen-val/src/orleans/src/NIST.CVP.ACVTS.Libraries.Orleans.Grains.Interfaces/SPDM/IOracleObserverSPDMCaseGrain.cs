using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.SPDM;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.SPDM;

public interface IOracleObserverSPDMCaseGrain : IGrainWithGuidKey, IGrainObservable<SPDMResult>
{
    Task<bool> BeginWorkAsync(SPDMParameters parameters);
}

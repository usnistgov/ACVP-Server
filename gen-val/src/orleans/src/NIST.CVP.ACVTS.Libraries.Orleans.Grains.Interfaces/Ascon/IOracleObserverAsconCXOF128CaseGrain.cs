using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Ascon;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Ascon;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Ascon;

public interface IOracleObserverAsconCXOF128CaseGrain : IGrainWithGuidKey, IGrainObservable<AsconHashResult>
{
    Task<bool> BeginWorkAsync(AsconHashParameters parameters);
}

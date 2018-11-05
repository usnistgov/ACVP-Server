using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using Orleans;

namespace NIST.CVP.Orleans.Grains.Interfaces.Kdf
{
    public interface IOracleObserverAnsiX963KdfCaseGrain : IGrainWithGuidKey, IGrainObservable<AnsiX963KdfResult>
    {
        Task<bool> BeginWorkAsync(AnsiX963Parameters param);
    }
}
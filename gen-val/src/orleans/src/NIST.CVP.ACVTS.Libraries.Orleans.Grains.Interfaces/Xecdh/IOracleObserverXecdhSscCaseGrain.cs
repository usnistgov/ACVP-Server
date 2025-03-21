using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using Orleans;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Interfaces.Xecdh
{
    public interface IOracleObserverXecdhSscCaseGrain : IGrainWithGuidKey, IGrainObservable<XecdhSscResult>
    {
        Task<bool> BeginWorkAsync(XecdhSscParameters param);
    }
}

using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KdfResult GetDeferredKdfCase(KdfParameters param);
        KdfResult CompleteDeferredKdfCase(KdfParameters param, KdfResult fullParam);
        
        // All the components individually probably, but those are straight-forward input/output

    }
}

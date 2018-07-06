using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        AeadResult GetAesCcmCase();
        AeadResult GetAesGcmCase(AeadParameters param);
        AeadResult GetAesXpnCase(AeadParameters param);

        AeadResult GetDeferredAesGcmCase(AeadParameters param);
        AeadResult CompleteDeferredAesGcmCase(AeadParameters param, AeadResult fullParam);
    }
}

using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        MacResult GetCmacCase(CmacParameters param);
        MacResult GetHmacCase(HmacParameters param);
    }
}

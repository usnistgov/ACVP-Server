using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        TdesResult GetTdesCase(TdesParameters param);
        TdesResultWithIvs GetTdesWithIvsCase(TdesParameters param);

        MctResult<TdesResult> GetTdesMctCase(TdesParameters param);
        MctResult<TdesResultWithIvs> GetTdesMctWithIvsCase(TdesParameters param);
    }
}

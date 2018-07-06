using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        TdesResult GetTdesCbcCase(TdesParameters param);
        TdesResult GetTdesCfbCase(TdesParameters param);
        TdesResult GetTdesEcbCase(TdesParameters param);
        TdesResult GetTdesOfbCase(TdesParameters param);

        TdesResultWithIvs GetTdesCbcICase(TdesParameters param);
        TdesResultWithIvs GetTdesOfbICase(TdesParameters param);

        MctResult<TdesResult> GetTdesCbcMctCase(TdesParameters param);
        MctResult<TdesResult> GetTdesCfbMctCase(TdesParameters param);
        MctResult<TdesResult> GetTdesEcbMctCase(TdesParameters param);
        MctResult<TdesResult> GetTdesOfbMctCase(TdesParameters param);

        MctResult<TdesResultWithIvs> GetTdesCbcIMctCase(TdesParameters param);
        MctResult<TdesResultWithIvs> GetTdesOfbIMctCase(TdesParameters param);
    }
}

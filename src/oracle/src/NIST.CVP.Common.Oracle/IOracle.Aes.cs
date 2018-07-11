using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Symmetric;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        AesResult GetAesCase(AesParameters param);
        MctResult<AesResult> GetAesMctCase(AesParameters param);

        AesResult GetDeferredAesCounterCase(CounterParameters<AesParameters> param);
        AesResult CompleteDeferredAesCounterCase(CounterParameters<AesParameters> param);
        CounterResult ExtractIvs(AesParameters param, AesResult fullParam);

        //AesResult GetAesCbcCase(AesParameters param);
        //AesResult GetAesCfbCase(AesParameters param);
        //AesResult GetAesCtrCase(AesParameters param);
        //AesResult GetAesEcbCase(AesParameters param);
        //AesResult GetAesOfbCase(AesParameters param);
        //AesResult GetAesXtsCase(AesParameters param);

        //MctResult<AesResult> GetAesCbcMctCase(AesParameters param);
        //MctResult<AesResult> GetAesCfbMctCase(AesParameters param);
        //MctResult<AesResult> GetAesEcbMctCase(AesParameters param);
        //MctResult<AesResult> GetAesOfbMctCase(AesParameters param);
    }
}

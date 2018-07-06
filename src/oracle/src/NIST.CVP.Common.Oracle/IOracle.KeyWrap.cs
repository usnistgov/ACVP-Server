using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        AesResult GetAesKeyWrapCase();
        AesResult GetAesKeyWrapWithPaddingCase();
        TdesResult GetTdesKeyWrapCase();
    }
}

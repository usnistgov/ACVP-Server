using NIST.CVP.Crypto.Common.DRBG;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        ResultTypes.DrbgResult GetDrbgCase(DrbgParameters param);
    }
}

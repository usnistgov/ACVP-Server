using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DsaKeyResult : IResult
    {
        public FfcKeyPair Key { get; set; }
    }
}

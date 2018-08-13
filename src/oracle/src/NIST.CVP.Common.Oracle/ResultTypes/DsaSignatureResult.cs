using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DsaSignatureResult
    {
        public BitString Message { get; set; }
        public FfcSignature Signature { get; set; }
        public FfcKeyPair Key { get; set; }
    }
}

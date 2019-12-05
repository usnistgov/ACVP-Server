using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class DsaSignatureResult
    {
        public BitString Message { get; set; }
        /// <summary>
        /// Used for 800-106 random message hashing
        /// </summary>
        public BitString RandomValue { get; set; }
        public FfcSignature Signature { get; set; }
        public FfcKeyPair Key { get; set; }
    }
}

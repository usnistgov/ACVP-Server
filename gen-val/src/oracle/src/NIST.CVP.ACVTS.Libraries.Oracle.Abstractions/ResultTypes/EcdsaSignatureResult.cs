using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class EcdsaSignatureResult
    {
        public BitString Message { get; set; }
        /// <summary>
        /// Used for 800-106 random message hashing
        /// </summary>
        public BitString RandomValue { get; set; }
        public EccKeyPair Key { get; set; }
        public EccSignature Signature { get; set; }
    }
}

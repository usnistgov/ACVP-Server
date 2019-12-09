using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class EddsaSignatureResult
    {
        public BitString Message { get; set; }
        public BitString Context { get; set; }
        public EdKeyPair Key { get; set; }
        public EdSignature Signature { get; set; }
    }
}

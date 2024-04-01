using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class EddsaSignatureResult
    {
        public BitString Message { get; set; }
        public BitString Context { get; set; }
        public int ContextLength { get; set; }
        public EdKeyPair Key { get; set; }
        public EdSignature Signature { get; set; }
    }
}

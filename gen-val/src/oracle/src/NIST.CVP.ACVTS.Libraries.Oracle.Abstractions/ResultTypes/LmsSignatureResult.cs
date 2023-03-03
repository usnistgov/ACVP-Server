using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class LmsSignatureResult
    {
        public BitString PublicKey { get; set; }
        public BitString Signature { get; set; }
        public BitString Message { get; set; }
    }
}

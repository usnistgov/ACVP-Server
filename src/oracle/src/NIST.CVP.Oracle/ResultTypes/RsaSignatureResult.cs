using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class RsaSignatureResult
    {
        public BitString Message { get; set; }
        public RsaKeyResult Key { get; set; }

        public BitString Signature { get; set; }
    }
}

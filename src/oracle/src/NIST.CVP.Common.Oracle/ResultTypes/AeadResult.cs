using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class AeadResult : AesResult, ICryptoResult
    {
        public bool TestPassed { get; set; }
        public BitString Aad { get; set; }
        public BitString Tag { get; set; }
    }
}

using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class AeadResult : AesResult, ICryptoResult
    {
        public bool TestPassed { get; set; }
        public BitString Aad { get; set; }
        public BitString Salt { get; set; }
        public BitString Tag { get; set; }
    }
}

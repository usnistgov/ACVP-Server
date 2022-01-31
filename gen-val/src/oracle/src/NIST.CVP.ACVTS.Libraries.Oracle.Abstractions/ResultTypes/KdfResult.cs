using NIST.CVP.ACVTS.Libraries.Crypto.Common;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class KdfResult : ICryptoResult
    {
        public BitString KeyIn { get; set; }
        public BitString Iv { get; set; }
        public BitString FixedData { get; set; }
        public int BreakLocation { get; set; }
        public BitString KeyOut { get; set; }
    }
}

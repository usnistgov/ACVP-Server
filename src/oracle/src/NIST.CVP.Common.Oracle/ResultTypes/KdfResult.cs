using NIST.CVP.Crypto.Common;
using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
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

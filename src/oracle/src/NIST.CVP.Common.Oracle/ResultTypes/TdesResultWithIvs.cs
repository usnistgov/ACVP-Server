using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class TdesResultWithIvs : TdesResult
    {
        public BitString Iv1 { get; set; }
        public BitString Iv2 { get; set; }
        public BitString Iv3 { get; set; }
    }
}

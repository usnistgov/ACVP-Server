using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes
{
    public class TdesResult
    {
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString Iv { get; set; }
        public BitString CipherText { get; set; }
    }
}

using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class TdesResult
    {
        public BitString PlainText { get; set; }
        public BitString Key1 { get; set; }
        public BitString Key2 { get; set; }
        public BitString Key3 { get; set; }
        public BitString Iv { get; set; }
        public BitString CipherText { get; set; }
    }
}

using NIST.CVP.Math;

namespace NIST.CVP.Common.Oracle.ResultTypes
{
    public class AesResult : Result
    {
        public BitString PlainText { get; set; }
        public BitString Key { get; set; }
        public BitString Iv { get; set; }
        public BitString CipherText { get; set; }
    }
}

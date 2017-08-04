using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public class AlgoArrayResponse
    {
        public BitString Keys { get; set; }
        public BitString IV { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
        public BitString Key1 { get; set; }
        public BitString Key2 { get; set; }
        public BitString Key3 { get; set; }
    }
}
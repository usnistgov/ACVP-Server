using NIST.CVP.Math;

namespace NIST.CVP.Generation.TDES
{
    public class AlgoArrayResponse
    {
        public BitString Keys { get; set; }
        public BitString IV { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
    }
}
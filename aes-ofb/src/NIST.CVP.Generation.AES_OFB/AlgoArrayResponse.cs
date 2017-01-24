using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_OFB
{
    public class AlgoArrayResponse
    {
        public BitString Key { get; set; }
        public BitString IV { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
    }
}
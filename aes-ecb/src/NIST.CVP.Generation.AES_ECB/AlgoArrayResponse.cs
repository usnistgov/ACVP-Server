using NIST.CVP.Math;

namespace NIST.CVP.Generation.AES_ECB
{
    public class AlgoArrayResponse
    {
        public BitString Key { get; set; }
        public BitString PlainText { get; set; }
        public BitString CipherText { get; set; }
    }
}

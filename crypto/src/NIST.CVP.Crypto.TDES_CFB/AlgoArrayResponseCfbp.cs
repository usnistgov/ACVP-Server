using NIST.CVP.Crypto.TDES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES_CFB
{
    public class AlgoArrayResponseCfbp : AlgoArrayResponse
    {
        public BitString PlainText2 { get; set; }
        public BitString PlainText3 { get; set; }
        public BitString CipherText2 { get; set; }
        public BitString CipherText3 { get; set; }
        public BitString IV2 { get; set; }
        public BitString IV3 { get; set; }
    }
}
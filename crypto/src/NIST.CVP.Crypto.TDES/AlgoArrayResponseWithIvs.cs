using NIST.CVP.Math;

namespace NIST.CVP.Crypto.TDES
{
    public class AlgoArrayResponseWithIvs : AlgoArrayResponse
    {

        public BitString IV1 { get; set; }
        public BitString IV2 { get; set; }
        public BitString IV3 { get; set; }
    }
}
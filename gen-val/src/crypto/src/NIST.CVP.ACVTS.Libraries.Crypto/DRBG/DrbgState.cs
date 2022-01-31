using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG
{
    public class DrbgState
    {
        public BitString LastEntropy { get; set; }
        public BitString LastNonce { get; set; }
    }
}

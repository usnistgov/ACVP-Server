using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DRBG
{
    public class DrbgState
    {
        public BitString LastEntropy { get; set; }
        public BitString LastNonce { get; set; }
    }
}

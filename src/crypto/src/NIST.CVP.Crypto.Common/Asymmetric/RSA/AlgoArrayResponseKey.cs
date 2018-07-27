using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA2
{
    public class AlgoArrayResponseKey
    {
        public BitString E { get; set; }
        public BitString P { get; set; }
        public BitString Q { get; set; }
        public bool FailureTest { get; set; }
    }
}

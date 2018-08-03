using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class AuxiliaryResult
    {
        public BigInteger XP { get; set; }
        public BigInteger XQ { get; set; }
        public BigInteger XP1 { get; set; }
        public BigInteger XQ1 { get; set; }
        public BigInteger XP2 { get; set; }
        public BigInteger XQ2 { get; set; }
    }
}

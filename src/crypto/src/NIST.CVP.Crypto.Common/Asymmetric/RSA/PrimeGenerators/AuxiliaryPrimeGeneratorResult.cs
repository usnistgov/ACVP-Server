using System.Numerics;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public class AuxiliaryPrimeGeneratorResult
    {
        public BigInteger XP1 { get; set; }
        public BigInteger XP2 { get; set; }
        public BigInteger XP { get; set; }
        public BigInteger XQ1 { get; set; }
        public BigInteger XQ2 { get; set; }
        public BigInteger XQ { get; set; }

        // B35
        public AuxiliaryPrimeGeneratorResult(BigInteger xp, BigInteger xq)
        {
            XP = xp;
            XQ = xq;
        }

        // B36
        public AuxiliaryPrimeGeneratorResult(BigInteger xp1, BigInteger xp2, BigInteger xp, BigInteger xq1, BigInteger xq2, BigInteger xq)
        {
            XP1 = xp1;
            XP2 = xp2;
            XP = xp;
            XQ1 = xq1;
            XQ2 = xq2;
            XQ = xq;
        }
    }
}

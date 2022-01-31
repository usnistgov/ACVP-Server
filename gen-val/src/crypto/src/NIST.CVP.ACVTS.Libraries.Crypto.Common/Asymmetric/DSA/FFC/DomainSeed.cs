using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC
{
    public class DomainSeed
    {
        public BigInteger Seed { get; set; }
        public BigInteger PSeed { get; set; }
        public BigInteger QSeed { get; set; }

        public PrimeGenMode Mode
        {
            get
            {
                if (PSeed != BigInteger.Zero && QSeed != BigInteger.Zero)
                {
                    return PrimeGenMode.Provable;
                }

                return PrimeGenMode.Probable;
            }
        }

        public DomainSeed()
        {

        }

        public DomainSeed(BigInteger seed)
        {
            Seed = seed;
        }

        public DomainSeed(BigInteger firstSeed, BigInteger pSeed, BigInteger qSeed)
        {
            Seed = firstSeed;
            PSeed = pSeed;
            QSeed = qSeed;
        }

        public BigInteger GetFullSeed()
        {
            var seedBS = new BitString(Seed).PadToModulusMsb(32);
            var pSeedBS = new BitString(PSeed).PadToModulusMsb(32);
            var qSeedBS = new BitString(QSeed).PadToModulusMsb(32);

            return BitString.ConcatenateBits(seedBS, BitString.ConcatenateBits(pSeedBS, qSeedBS)).ToPositiveBigInteger();
        }

        public void ModifySeed(BigInteger newSeed)
        {
            Seed = newSeed;
        }
    }
}

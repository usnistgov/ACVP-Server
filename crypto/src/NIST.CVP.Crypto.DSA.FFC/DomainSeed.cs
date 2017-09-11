using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NIST.CVP.Crypto.DSA.FFC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public class DomainSeed
    {
        public BigInteger Seed { get; private set; }
        public BigInteger PSeed { get; private set; }
        public BigInteger QSeed { get; private set; }
        public PrimeGenMode Mode { get; private set; }

        public DomainSeed(BigInteger seed)
        {
            Seed = seed;
            Mode = PrimeGenMode.Probable;
        }

        public DomainSeed(BigInteger firstSeed, BigInteger pSeed, BigInteger qSeed)
        {
            Seed = firstSeed;
            PSeed = pSeed;
            QSeed = qSeed;
            Mode = PrimeGenMode.Provable;
        }

        public BigInteger GetFullSeed()
        {
            var seedBS = new BitString(Seed);
            var pSeedBS = new BitString(PSeed);
            var qSeedBS = new BitString(QSeed);

            return BitString.ConcatenateBits(seedBS, BitString.ConcatenateBits(pSeedBS, qSeedBS)).ToPositiveBigInteger();
        }
    }
}

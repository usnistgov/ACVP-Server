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
        private readonly BigInteger Seed;
        private readonly BigInteger PSeed;
        private readonly BigInteger QSeed;
        private readonly PrimeGenMode Mode;

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

        public BigInteger GetSeed()
        {
            var seedBS = new BitString(Seed);
            var pSeedBS = new BitString(PSeed);
            var qSeedBS = new BitString(QSeed);

            return BitString.ConcatenateBits(seedBS, BitString.ConcatenateBits(pSeedBS, qSeedBS)).ToPositiveBigInteger();
        }
    }
}

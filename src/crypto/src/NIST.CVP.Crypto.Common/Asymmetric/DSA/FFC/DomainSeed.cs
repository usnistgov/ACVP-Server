using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC
{
    public class DomainSeed
    {
        public BitString Seed { get; set; }
        public BitString PSeed { get; set; }
        public BitString QSeed { get; set; }

        public PrimeGenMode Mode
        {
            get
            {
                if (PSeed != null && QSeed != null)
                {
                    return PrimeGenMode.Provable;
                }

                return PrimeGenMode.Probable;
            }
        }

        public DomainSeed()
        {
            
        }

        public DomainSeed(BitString seed)
        {
            Seed = seed;
        }

        public DomainSeed(BitString firstSeed, BitString pSeed, BitString qSeed)
        {
            Seed = firstSeed;
            PSeed = pSeed;
            QSeed = qSeed;
        }

        public BitString GetFullSeed()
        {
            var emptyBitString = new BitString(0);
            
            return new BitString(0)
                .ConcatenateBits(Seed ?? emptyBitString)
                .ConcatenateBits(PSeed ?? emptyBitString)
                .ConcatenateBits(QSeed ?? emptyBitString);
        }

        public void ModifySeed(BitString newSeed)
        {
            Seed = newSeed;
        }
    }
}

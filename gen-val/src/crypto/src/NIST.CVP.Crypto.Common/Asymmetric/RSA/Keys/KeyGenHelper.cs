using System.Numerics;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.Keys
{
    public static class KeyGenHelper
    {
        private static readonly IRandom800_90 _rand = new Random800_90();
        
        public static BitString GetEValue(Fips186Standard standard, int minLen, int maxLen)
        {
            if (standard == Fips186Standard.Fips186_2)
            {
                var exp = new BigInteger[] {3, 17, 65537};
                return new BitString(exp[_rand.GetRandomInt(0, 3)]);
            }
            
            BigInteger e;
            BitString e_bs;
            do
            {
                var min = minLen / 2;
                var max = maxLen / 2;

                e = GetRandomBigIntegerOfBitLength(_rand.GetRandomInt(min, max) * 2);
                if (e.IsEven)
                {
                    e++;
                }

                e_bs = new BitString(e);
            } while (e_bs.BitLength >= maxLen || e_bs.BitLength < minLen);

            return new BitString(e);
        }
        
        public static BitString GetSeed(int modulo)
        {
            var security_strength = 0;
            if(modulo == 1024)
            {
                security_strength = 80;
            }
            else if (modulo == 2048)
            {
                security_strength = 112;
            }
            else if (modulo == 3072)
            {
                security_strength = 128;
            }
            else if (modulo == 4096)
            {
                security_strength = 128;
            }

            return _rand.GetRandomBitString(2 * security_strength);
        }

        public static int[] GetBitlens(int modulo, PrimeGenModes mode)
        {
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if(modulo == 1024)
            {
                // Rough estimate based on existing test vectors
                min_single = 101;
                max_both = 236;
            }
            else if (modulo == 2048)
            {
                min_single = 140 + 1;

                if (mode == PrimeGenModes.RandomProvablePrimes || mode == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                {
                    max_both = 494;
                }
                else
                {
                    max_both = 750;
                }
            }
            else if (modulo == 3072)
            {
                min_single = 170 + 1;

                if (mode == PrimeGenModes.RandomProvablePrimes || mode == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                {
                    max_both = 750;
                }
                else
                {
                    max_both = 1518;
                }
            }
            else if (modulo == 4096)
            {
                min_single = 200 + 1;

                if (mode == PrimeGenModes.RandomProvablePrimes || mode == PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes)
                {
                    max_both = 1005;
                }
                else
                {
                    max_both = 2030;
                }
            }

            bitlens[0] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = _rand.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = _rand.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = _rand.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }
        
        private static BigInteger GetRandomBigIntegerOfBitLength(int len)
        {
            var bs = _rand.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }
    }
}
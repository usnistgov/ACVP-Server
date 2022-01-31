using System;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys
{
    public static class KeyGenHelper
    {
        public static int CalculateEstimatedSecurityStrength(int modulo)
        {
            // a = 1/3
            var a = (double)1 / 3;

            // b = 2/3
            var b = (double)2 / 3;

            // t = l( l(2 ^ c) )
            var t = BigInteger.Log((BigInteger)BigInteger.Log(BigInteger.Pow(2, modulo)));

            // # if b < 1, then a^b == e (l(a) * b)
            // m = e( l(t) * b )
            var m = System.Math.Exp(System.Math.Log(t) * b);

            // t = 64 / 9 * l(2 ^ c)
            t = (double)64 / 9 * BigInteger.Log(BigInteger.Pow(2, modulo));

            // n = e( l(t) * a )
            var n = System.Math.Exp(System.Math.Log(t) * a);

            // o = e( m * n )
            var o = System.Math.Exp(m * n);

            // p = l(o) / l(2)
            return (int)(System.Math.Log(o) / System.Math.Log(2));
        }

        public static int GetEstimatedSecurityStrength(int modulo)
        {
            /*
                From: https://nvlpubs.nist.gov/nistpubs/SpecialPublications/NIST.SP.800-57pt1r5.pdf
             
                80        1024
                112       2048
                128       3072
                192       7680
                256       15360
             */

            return modulo switch
            {
                >= 1024 and < 2048 => 80,
                >= 2048 and < 3072 => 112,
                >= 3072 and < 7680 => 128,
                >= 7680 and < 15360 => 192,
                15360 => 256,
                _ => throw new ArgumentOutOfRangeException(nameof(modulo))
            };
        }

        public static BitString GetEValue(IRandom800_90 random, Fips186Standard standard, int minLen, int maxLen)
        {
            if (standard == Fips186Standard.Fips186_2)
            {
                var exp = new BigInteger[] { 3, 17, 65537 };
                return new BitString(exp[random.GetRandomInt(0, 3)]);
            }

            BigInteger e;
            BitString e_bs;
            do
            {
                var min = minLen / 2;
                var max = maxLen / 2;

                e = GetRandomBigIntegerOfBitLength(random, random.GetRandomInt(min, max) * 2);
                if (e.IsEven)
                {
                    e++;
                }

                e_bs = new BitString(e);
            } while (e_bs.BitLength >= maxLen || e_bs.BitLength < minLen);

            return new BitString(e);
        }

        public static BitString GetSeed(IRandom800_90 random, int modulo)
        {
            return random.GetRandomBitString(2 * GetEstimatedSecurityStrength(modulo));
        }

        public static int[] GetBitlens(IRandom800_90 random, int modulo, PrimeGenModes mode)
        {
            var bitlens = new int[4];
            var min_single = 0;
            var max_both = 0;

            // Min_single values were given as exclusive, we add 1 to make them inclusive
            if (modulo == 1024)
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
            else if (modulo >= 4096)
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

            bitlens[0] = random.GetRandomInt(min_single, max_both - min_single);
            bitlens[1] = random.GetRandomInt(min_single, max_both - bitlens[0]);
            bitlens[2] = random.GetRandomInt(min_single, max_both - min_single);
            bitlens[3] = random.GetRandomInt(min_single, max_both - bitlens[2]);

            return bitlens;
        }

        private static BigInteger GetRandomBigIntegerOfBitLength(IRandom800_90 random, int len)
        {
            var bs = random.GetRandomBitString(len);
            return bs.ToPositiveBigInteger();
        }
    }
}

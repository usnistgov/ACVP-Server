using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Math;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public static class PrimeGeneratorGuard
    {
        public static int[] ValidModulusFips186_2 = { 1024, 1536, 2048, 3072, 4096 };
        public static int[] ValidModulusFips186_4 = { 1024, 2048, 3072, 4096 }; // Allowing 1024 modulo for key gen for sigver
        public static int[] ValidModulusFips186_5 = { 2048, 3072, 4096, 6144, 8192 };
        //public static int[] ValidModulusFips186_5 = { 2048, 3072, 4096, 6144, 8192, 15360 }; - Disabled 15360 temporarily while pools fill - 5+ months
        public static BigInteger MinValidE = NumberTheory.Pow2(16);
        public static BigInteger MaxValidE = NumberTheory.Pow2(256);
        public static int[] ValidAB = { 1, 3, 5, 7 };

        public static void AgainstInvalidAB(int ab)
        {
            if (!ValidAB.Contains(ab) && ab != default(int))
            {
                throw new RsaPrimeGenException($"Incorrect {nameof(ab)} value, must be 1, 3, 5, 7, or 0");
            }
        }

        public static void AgainstInvalidModulusFips186_2(int modulus)
        {
            AgainstInvalidModulus(ValidModulusFips186_2, modulus);
        }

        public static void AgainstInvalidModulusFips186_4(int modulus)
        {
            AgainstInvalidModulus(ValidModulusFips186_4, modulus);
        }

        public static void AgainstInvalidModulusFips186_5(int modulus)
        {
            AgainstInvalidModulus(ValidModulusFips186_5, modulus);
        }

        private static void AgainstInvalidModulus(IEnumerable<int> validModulus, int modulus)
        {
            if (!validModulus.Contains(modulus))
            {
                throw new RsaPrimeGenException($"{nameof(modulus)}: {modulus} is invalid");
            }
        }

        public static void AgainstInvalidPublicExponent(BigInteger e)
        {
            if (e <= MinValidE || e >= MaxValidE || e.IsEven)
            {
                throw new RsaPrimeGenException($"Incorrect {nameof(e)}, must be greater than 2^16, less than 2^256, odd");
            }
        }

        public static void AgainstInvalidPublicExponentFips186_2(BigInteger e)
        {
            if (e != 3 && e != 17 && e != 65537)
            {
                throw new RsaPrimeGenException($"Incorrect {nameof(e)}, must be 3, 17, 65537");
            }
        }

        public static void AgainstInvalidSeed(int modulus, BitString seed)
        {
            // If not a valid modulus, don't look up the security strength
            if (!(ValidModulusFips186_2.Contains(modulus) || ValidModulusFips186_4.Contains(modulus) ||
                ValidModulusFips186_5.Contains(modulus)))
            {
                return;
            }

            if (seed.BitLength < 2 * KeyGenHelper.GetEstimatedSecurityStrength(modulus))
            {
                throw new RsaPrimeGenException($"Invalid {nameof(seed)} length");
            }
        }

        public static void AgainstInvalidBitlens(int modulus, int[] bitlens)
        {
            if (bitlens == null)
            {
                throw new RsaPrimeGenException($"Invalid {nameof(bitlens)} is null");
            }

            if (bitlens.Count() != 4)
            {
                throw new RsaPrimeGenException($"Invalid {nameof(bitlens)}, must have 4 elements");
            }

            foreach (var bitlen in bitlens)
            {
                if (bitlen <= 0)
                {
                    throw new RsaPrimeGenException($"Invalid {nameof(bitlen)} provided, must be >= 0");
                }
            }
        }
    }
}

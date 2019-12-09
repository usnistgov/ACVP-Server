using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NIST.CVP.Crypto.Common.Math;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Asymmetric.RSA.PrimeGenerators
{
    public static class PrimeGeneratorGuard
    {
        public static int[] ValidModulusFips186_2 = {1024, 1536, 2048, 3072, 4096};
        public static int[] ValidModulusFips186_4 = {1024, 2048, 3072, 4096}; // Allowing 1024 modulo for key gen for sigver
        public static int[] ValidModulusFips186_5 = {2048, 3072, 4096, 8192, 15360};
        public static BigInteger MinValidE = NumberTheory.Pow2(16);
        public static BigInteger MaxValidE = NumberTheory.Pow2(256);
        public static int[] ValidAB = {1, 3, 5, 7};
        
        public static void AgainstInvalidAB(int ab, List<string> errors)
        {
            if (!ValidAB.Contains(ab) && ab != default(int))
            {
                errors.Add($"Incorrect {nameof(ab)} value, must be 1, 3, 5, 7, or 0");
            }
        }
        
        public static void AgainstInvalidModulusFips186_2(int modulus, List<string> errors)
        {
            AgainstInvalidModulus(ValidModulusFips186_2, modulus, errors);
        }
        
        public static void AgainstInvalidModulusFips186_4(int modulus, List<string> errors)
        {
            AgainstInvalidModulus(ValidModulusFips186_4, modulus, errors);
        }

        public static void AgainstInvalidModulusFips186_5(int modulus, List<string> errors)
        {
            AgainstInvalidModulus(ValidModulusFips186_5, modulus, errors);
        }

        private static void AgainstInvalidModulus(IEnumerable<int> validModulus, int modulus, ICollection<string> errors)
        {
            if (!validModulus.Contains(modulus))
            {
                errors.Add($"{nameof(modulus)}: {modulus} is invalid");
            }
        }

        public static void AgainstInvalidPublicExponent(BigInteger e, List<string> errors)
        {
            if (e <=  MinValidE || e >= MaxValidE || e.IsEven)
            {
                errors.Add($"Incorrect {nameof(e)}, must be greater than 2^16, less than 2^256, odd");
            }
        }
        
        public static void AgainstInvalidPublicExponentFips186_2(BigInteger e, List<string> errors)
        {
            // TODO check these requirements for e
            if (e != 3 && e != 17 && e != 65537)
            {
                errors.Add($"Incorrect {nameof(e)}, must be 3, 17, 65537");
            }
        }

        public static void AgainstInvalidSeed(int modulus, BitString seed, List<string> errors)
        {
            // If not a valid modulus, don't look up the security strength
            if (!(ValidModulusFips186_2.Contains(modulus) || ValidModulusFips186_4.Contains(modulus) ||
                ValidModulusFips186_5.Contains(modulus)))
            {
                return;
            }
            
            if (seed.BitLength != 2 * GetSecurityStrengthFromModulus(modulus))
            {
                errors.Add($"Invalid {nameof(seed)} length");
            }
        }

        public static void AgainstInvalidBitlens(int modulus, int[] bitlens, List<string> errors)
        {
            if (bitlens == null)
            {
                errors.Add($"Invalid {nameof(bitlens)} is null");
                return;
            }
            
            if (bitlens.Count() != 4)
            {
                errors.Add($"Invalid {nameof(bitlens)}, must have 4 elements");
            }

            errors.AddRange(from bitlen in bitlens where bitlen <= 0 select $"Invalid {nameof(bitlen)} provided, must be >= 0");
        }
        
        // TODO remove this duplicated method, but it is needed because the normal method is within PrimeGeneratorHelper in Crypto, not Common.Crypto
        private static int GetSecurityStrengthFromModulus(int modulus)
        {
            switch (modulus)
            {
                case 1024:        // limited support
                    return 80;
                
                case 2048:
                    return 112;
                
                case 3072:
                    return 128;
                
                case 4096:
                    return 128;
                
                default:
                    throw new ArgumentException($"{nameof(modulus)} provided is invalid: {modulus}.");
            }
        }
    }
}
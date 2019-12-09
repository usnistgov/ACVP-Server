using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public static class RsaKeyGenAttributeConverter
    {
        private static readonly List<(PrimeGenModes primeGen, PrimeGenFips186_4Modes section)> _primeGenAttributes = new List<(PrimeGenModes, PrimeGenFips186_4Modes)>
        {
            (PrimeGenModes.RandomProvablePrimes, PrimeGenFips186_4Modes.B32),
            (PrimeGenModes.RandomProbablePrimes, PrimeGenFips186_4Modes.B33),
            (PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes, PrimeGenFips186_4Modes.B34),
            (PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes, PrimeGenFips186_4Modes.B35),
            (PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes, PrimeGenFips186_4Modes.B36)
        };
        
        private static readonly List<(PrimeTestModes primeTest, PrimeTestFips186_4Modes section)> _primeTestAttributes = new List<(PrimeTestModes, PrimeTestFips186_4Modes)>
        {
            (PrimeTestModes.TwoPow100ErrorBound, PrimeTestFips186_4Modes.TblC3),
            (PrimeTestModes.TwoPowSecurityStrengthErrorBound, PrimeTestFips186_4Modes.TblC2),
            (PrimeTestModes.Invalid, PrimeTestFips186_4Modes.Invalid)
        };

        public static PrimeGenModes GetPrimeGenFromSection(PrimeGenFips186_4Modes section)
        {
            if (!_primeGenAttributes.TryFirst(w => w.section == section, out var result))
            {
                throw new ArgumentException($"{nameof(section)} provided does not exist");
            }
            
            return result.primeGen;
        }

        public static PrimeGenFips186_4Modes GetSectionFromPrimeGen(PrimeGenModes primeGen, bool shouldThrow = true)
        {
            if (!_primeGenAttributes.TryFirst(w => w.primeGen == primeGen, out var result))
            {
                if (shouldThrow)
                {
                    throw new ArgumentException($"{nameof(primeGen)} provided does not exist");
                }

                return PrimeGenFips186_4Modes.Invalid;
            }
            
            return result.section;
        }
        
        public static PrimeTestModes GetPrimeTestFromSection(PrimeTestFips186_4Modes section)
        {
            if (!_primeTestAttributes.TryFirst(w => w.section == section, out var result))
            {
                throw new ArgumentException($"{nameof(section)} provided does not exist");
            }
            
            return result.primeTest;
        }

        public static PrimeTestFips186_4Modes GetSectionFromPrimeTest(PrimeTestModes primeTest, bool shouldThrow = true)
        {
            if (!_primeTestAttributes.TryFirst(w => w.primeTest == primeTest, out var result))
            {
                if (shouldThrow)
                {
                    throw new ArgumentException($"{nameof(primeTest)} provided does not exist");
                }

                return PrimeTestFips186_4Modes.Invalid;
            }
            
            return result.section;
        }
    }
}
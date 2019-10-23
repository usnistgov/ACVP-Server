using System;
using System.Collections.Generic;
using NIST.CVP.Common.ExtensionMethods;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;

namespace NIST.CVP.Generation.RSA.v1_0.KeyGen
{
    public static class RsaKeyGenAttributeConverter
    {
        private static readonly List<(PrimeGenModes primeGen, string section)> _primeGenAttributes = new List<(PrimeGenModes, string)>
        {
            (PrimeGenModes.RandomProvablePrimes, "B.3.2"),
            (PrimeGenModes.RandomProbablePrimes, "B.3.3"),
            (PrimeGenModes.RandomProvablePrimesWithAuxiliaryProvablePrimes, "B.3.4"),
            (PrimeGenModes.RandomProbablePrimesWithAuxiliaryProvablePrimes, "B.3.5"),
            (PrimeGenModes.RandomProbablePrimesWithAuxiliaryProbablePrimes, "B.3.6")
        };
        
        private static readonly List<(PrimeTestModes primeTest, string section, string firehoseTest)> _primeTestAttributes = new List<(PrimeTestModes, string, string)>
        {
            (PrimeTestModes.TwoPow100ErrorBound, "tblC2", "c.2"),
            (PrimeTestModes.TwoPowSecurityStrengthErrorBound, "tblC3", "c.3")
        };

        public static PrimeGenModes GetPrimeGenFromString(string section)
        {
            if (!_primeGenAttributes.TryFirst(w => w.section.Equals(section, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                throw new ArgumentException($"{nameof(section)} provided does not exist");
            }
            
            return result.primeGen;
        }

        public static string GetStringFromPrimeGen(PrimeGenModes primeGen)
        {
            if (!_primeGenAttributes.TryFirst(w => w.primeGen == primeGen,
                out var result))
            {
                throw new ArgumentException($"{nameof(primeGen)} provided does not exist");
            }
            
            return result.section;
        }
        
        public static PrimeTestModes GetPrimeTestFromString(string section)
        {
            if (!_primeTestAttributes.TryFirst(w => w.section.Equals(section, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                throw new ArgumentException($"{nameof(section)} provided does not exist");
            }
            
            return result.primeTest;
        }

        public static PrimeTestModes GetPrimeTestFromFirehose(string firehose)
        {
            if (!_primeTestAttributes.TryFirst(w => w.firehoseTest.Equals(firehose, StringComparison.OrdinalIgnoreCase),
                out var result))
            {
                throw new ArgumentException($"{nameof(firehose)} provided does not exist");
            }
            
            return result.primeTest;    
        }

        public static string GetStringFromPrimeTest(PrimeTestModes primeTest)
        {
            if (!_primeTestAttributes.TryFirst(w => w.primeTest == primeTest,
                out var result))
            {
                throw new ArgumentException($"{nameof(primeTest)} provided does not exist");
            }
            
            return result.section;
        }
    }
}
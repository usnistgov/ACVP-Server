using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.PrimeGenerators;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NLog;

namespace NIST.CVP.Generation.RSA_KeyGen
{
    public class TestCaseGeneratorAFT_B32 : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 25;

        private readonly IRandom800_90 _random800_90;
        private readonly RandomProvablePrimeGenerator _primeGen;

        public TestCaseGeneratorAFT_B32(IRandom800_90 random800_90, RandomProvablePrimeGenerator primeGen)
        {
            _random800_90 = random800_90;
            _primeGen = primeGen;
        }

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGenerateResponse Generate(TestGroup @group, bool isSample)
        {
            if (isSample)
            {
                _numberOfCases = 3;
            }

            if (group.InfoGeneratedByServer)
            {
                var testCase = new TestCase();

                // Generate E
                // E must be ODD and 2^16 < e < 2^256
                var e = _random800_90.GetRandomBigInteger(BigInteger.Pow(2, 16) + 1, BigInteger.Pow(2, 256) - 1);
                if (e.IsEven)
                {
                    e++;
                }

                // Generate Seed
                // Seed length must be 2 * security_strength
                var security_strength = 0;
                if (group.Modulo == 2048)
                {
                    security_strength = 112;
                }
                else if (group.Modulo == 3072)
                {
                    security_strength = 128;
                }

                var seed = _random800_90.GetRandomBitString(2 * security_strength);

                // Generate TestCase
                testCase.Key = new KeyPair {PubKey = new PublicKey {E = e}};
                testCase.Seed = seed;
                return Generate(group, testCase);
            }
            else
            {
                var testCase = new TestCase();
                testCase.Deferred = true;
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup @group, TestCase testCase)
        {
            PrimeGeneratorResult primeResult = null;
            try
            {
                // Configure Prime Generator
                _primeGen.SetHashFunction(group.HashAlg);
                _primeGen.SetEntropyProviderType(EntropyProviderTypes.Random);

                primeResult = _primeGen.GeneratePrimes(group.Modulo, testCase.Key.PubKey.E, testCase.Seed.GetDeepCopy());
                if (!primeResult.Success)
                {
                    ThisLogger.Warn(primeResult.ErrorMessage);
                    return new TestCaseGenerateResponse(primeResult.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error(ex);
                return new TestCaseGenerateResponse(ex.Message);
            }

            // Set p, q, d (and CRT d values), n, e in the testCase
            testCase.Key = new KeyPair(primeResult.P, primeResult.Q, testCase.Key.PubKey.E);
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

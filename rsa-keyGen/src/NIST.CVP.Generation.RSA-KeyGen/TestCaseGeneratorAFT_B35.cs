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
    public class TestCaseGeneratorAFT_B35 : ITestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 25;

        private readonly IRandom800_90 _random800_90;
        private readonly ProvableProbablePrimesWithConditionsGenerator _primeGen;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorAFT_B35(IRandom800_90 random800_90, ProvableProbablePrimesWithConditionsGenerator primeGen)
        {
            _random800_90 = random800_90;
            _primeGen = primeGen;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                _numberOfCases = 3;
            }

            if (group.InfoGeneratedByServer)
            {
                var e = TestCaseGeneratorHelper.GetEValue(group, _random800_90, BigInteger.Pow(2, 16) + 1, BigInteger.Pow(2, 256));
                var seed = TestCaseGeneratorHelper.GetSeed(group, _random800_90);
                var bitlens = TestCaseGeneratorHelper.GetBitlens(group, _random800_90);

                // Generate TestCase
                var testCase = new TestCase
                {
                    Key = new KeyPair { PubKey = new PublicKey { E = e } },
                    Seed = seed,
                    Bitlens = bitlens
                };
                return Generate(group, testCase);
            }
            else
            {
                var testCase = TestCaseGeneratorHelper.GetEmptyTestCase(group);
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            PrimeGeneratorResult primeResult = null;
            try
            {
                // Configure Prime Generator
                _primeGen.SetHashFunction(group.HashAlg);
                _primeGen.SetBitlens(testCase.Bitlens);
                _primeGen.SetEntropyProviderType(EntropyProviderTypes.Random);
                _primeGen.SetPrimeTestMode(group.PrimeTest);

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

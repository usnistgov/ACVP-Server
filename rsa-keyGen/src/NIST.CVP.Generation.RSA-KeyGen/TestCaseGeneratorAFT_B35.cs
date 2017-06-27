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
    public class TestCaseGeneratorAFT_B35 : IDeferredTestCaseGenerator<TestGroup, TestCase>
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

            // Set auxiliary values
            testCase.XP = primeResult.AuxValues.XP;
            testCase.XQ = primeResult.AuxValues.XQ;

            return new TestCaseGenerateResponse(testCase);
        }

        public TestCaseGenerateResponse CompleteDeferredTestCase(TestGroup group, TestCase testCase)
        {
            PrimeGeneratorResult primeResult = null;
            try
            {
                // Configure Prime Generator
                _primeGen.SetHashFunction(group.HashAlg);
                _primeGen.SetBitlens(testCase.Bitlens);
                _primeGen.SetPrimeTestMode(group.PrimeTest);
                _primeGen.SetEntropyProviderType(EntropyProviderTypes.Testable);

                // Set 'random' values with the values the client provided
                _primeGen.AddEntropy(testCase.XP);
                _primeGen.AddEntropy(testCase.XQ);

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

        public TestCaseGenerateResponse RecombineTestCases(TestGroup group, TestCase suppliedResult,
            TestCase originalTestCase)
        {
            if (suppliedResult.TestCaseId != originalTestCase.TestCaseId)
            {
                return new TestCaseGenerateResponse($"Mismatch TestCaseIds for TestCase: {suppliedResult.TestCaseId}");
            }

            if (group.PubExp == PubExpModes.FIXED)
            {
                if (suppliedResult.Key.PubKey.E != originalTestCase.Key.PubKey.E)
                {
                    return new TestCaseGenerateResponse($"Mismatch E value for TestCase: {suppliedResult.TestCaseId}");
                }
            }

            if (!TestCaseGeneratorHelper.ValidateBitlens(group, suppliedResult.Bitlens))
            {
                return new TestCaseGenerateResponse($"Improper bitlen values for TestCase: {suppliedResult.TestCaseId}");
            }

            var combinedTestCase = new TestCase
            {
                TestCaseId = suppliedResult.TestCaseId,
                Key = new KeyPair {PubKey = new PublicKey {E = originalTestCase.Key.PubKey.E}},
                Seed = suppliedResult.Seed,
                Bitlens = suppliedResult.Bitlens,
                XP = suppliedResult.XP,
                XQ = suppliedResult.XQ
            };
            
            return new TestCaseGenerateResponse(combinedTestCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

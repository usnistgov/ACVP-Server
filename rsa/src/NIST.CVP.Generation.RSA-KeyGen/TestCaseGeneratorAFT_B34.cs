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
    public class TestCaseGeneratorAFT_B34 : IDeferredTestCaseGenerator<TestGroup, TestCase>
    {
        private int _numberOfCases = 25;

        private readonly IRandom800_90 _random800_90;
        private readonly AllProvablePrimesWithConditionsGenerator _primeGen;

        public int NumberOfTestCasesToGenerate { get { return _numberOfCases; } }

        public TestCaseGeneratorAFT_B34(IRandom800_90 random800_90, AllProvablePrimesWithConditionsGenerator primeGen)
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

            if (group.InfoGeneratedByServer || isSample)
            {
                BigInteger e;
                if (group.PubExp == PubExpModes.FIXED)
                {
                    e = group.FixedPubExp.ToPositiveBigInteger();
                }
                else if (group.PubExp == PubExpModes.RANDOM)
                {
                    e = RSAEnumHelpers.GetEValue();
                }

                var seed = RSAEnumHelpers.GetSeed(group.Modulo);
                var bitlens = RSAEnumHelpers.GetBitlens(group.Modulo, group.Mode);

                // Generate TestCase
                var testCase = new TestCase
                {
                    Key = new KeyPair {PubKey = new PublicKey {E = e}},
                    Seed = seed,
                    Bitlens = bitlens,
                    Deferred = !group.InfoGeneratedByServer
                };

                // Generation can fail, recursive call until success
                var generateResponse = Generate(group, testCase);
                if (generateResponse.Success)
                {
                    return generateResponse;
                }

                return Generate(group, isSample);
            }
            else
            {
                var testCase = TestCase.GetEmptyTestCase(group);
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
                _primeGen.SetBitlens(testCase.Bitlens);
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

        public TestCaseGenerateResponse CompleteDeferredTestCase(TestGroup group, TestCase testCase)
        {
            PrimeGeneratorResult primeResult = null;
            try
            {
                // Configure Prime Generator
                _primeGen.SetHashFunction(group.HashAlg);
                _primeGen.SetBitlens(testCase.Bitlens);
                _primeGen.SetEntropyProviderType(EntropyProviderTypes.Testable);

                // No entropy values to enter as there is no RNG used

                primeResult = _primeGen.GeneratePrimes(group.Modulo, testCase.Key.PubKey.E, testCase.Seed.GetDeepCopy());
                if (!primeResult.Success)
                {
                    ThisLogger.Warn($"Deferred error: {primeResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Deferred error: {primeResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Deferred exception: {ex.Message}");
                return new TestCaseGenerateResponse($"Deferred exception: {ex.Message}");
            }

            // Set remaining values
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

            if (!RSAEnumHelpers.ValidateBitlens(group.Modulo, group.Mode, suppliedResult.Bitlens))
            {
                return new TestCaseGenerateResponse($"Improper bitlen values for TestCase: {suppliedResult.TestCaseId}");
            }

            var combinedTestCase = new TestCase
            {
                TestCaseId = suppliedResult.TestCaseId,
                Key = new KeyPair { PubKey = new PublicKey { E = suppliedResult.Key.PubKey.E } },
                Seed = suppliedResult.Seed,
                Bitlens = suppliedResult.Bitlens
            };

            return new TestCaseGenerateResponse(combinedTestCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

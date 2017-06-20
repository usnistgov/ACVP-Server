using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class TestCaseGeneratorGDT_B33 : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly RandomProbablePrimeGenerator _primeGen;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGeneratorGDT_B33(IRandom800_90 random800_90, RandomProbablePrimeGenerator primeGen)
        {
            _random800_90 = random800_90;
            _primeGen = primeGen;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var testCase = TestCaseGeneratorHelper.GetEmptyTestCase(group);

            if (isSample)
            {
                var e = TestCaseGeneratorHelper.GetEValue(group, _random800_90, BigInteger.Pow(2, 16) + 1, BigInteger.Pow(2, 256));
                testCase.Key = new KeyPair {PubKey = new PublicKey {E = e}};
                return Generate(group, testCase);
            }

            return new TestCaseGenerateResponse(testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            PrimeGeneratorResult primeResult = null;
            try
            {
                // Configure Prime Generator
                _primeGen.SetEntropyProviderType(EntropyProviderTypes.Random);

                primeResult = _primeGen.GeneratePrimes(group.Modulo, testCase.Key.PubKey.E, null);
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

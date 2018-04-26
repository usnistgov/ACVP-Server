using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaFfcFactory _dsaFactory;
        private IDsaFfc _ffcDsa;

        public int NumberOfTestCasesToGenerate => 10;

        public TestCaseGenerator(IRandom800_90 rand, IDsaFfcFactory dsaFactory)
        {
            _random = rand;
            _dsaFactory = dsaFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                return Generate(group, new TestCase());
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(new TestCase());
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            FfcKeyPairGenerateResult keyResult = null;
            try
            {
                var hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                _ffcDsa = _dsaFactory.GetInstance(hashFunction);
                keyResult = _ffcDsa.GenerateKeyPair(group.DomainParams);
                if (!keyResult.Success)
                {
                    ThisLogger.Warn($"Error generating key: {keyResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating key: {keyResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating key: {ex.StackTrace}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating key: {ex.Message}");
            }

            testCase.Key = keyResult.KeyPair;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

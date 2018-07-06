using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaFfcFactory _dsaFactory;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGenerator(IRandom800_90 rand, IDsaFfcFactory ffcDsaFactory)
        {
            _random = rand;
            _dsaFactory = ffcDsaFactory;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase
            {
                Message = _random.GetRandomBitString(group.L)
            };

            if (isSample)
            {
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
            }
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            var ffcDsa = _dsaFactory.GetInstance(group.HashAlg);

            // Generate a key
            FfcKeyPairGenerateResult keyResult = null;
            try
            {
                keyResult = ffcDsa.GenerateKeyPair(group.DomainParams);
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

            // Generate the signature
            FfcSignatureResult sigResult = null;
            try
            {
                sigResult = ffcDsa.Sign(group.DomainParams, testCase.Key, testCase.Message);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature: {ex.Message}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating signature: {ex.Message}");
            }

            testCase.Signature = sigResult.Signature;
            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

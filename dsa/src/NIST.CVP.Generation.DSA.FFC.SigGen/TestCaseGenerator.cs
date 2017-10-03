using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.SigGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaFfc _ffcDsa;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGenerator(IRandom800_90 rand, IDsaFfc ffcDsa)
        {
            _random = rand;
            _ffcDsa = ffcDsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase()
            {
                Message = _random.GetRandomBitString(group.L)
            };

            if (isSample)
            {
                return Generate(group, testCase);
            }
            else
            {
                return new TestCaseGenerateResponse(testCase);
            }
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            // Generate a key
            FfcKeyPairGenerateResult keyResult = null;
            try
            {
                keyResult = _ffcDsa.GenerateKeyPair(group.DomainParams);
                if (!keyResult.Success)
                {
                    ThisLogger.Warn($"Error generating key: {keyResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating key: {keyResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating key: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating key: {ex.Message}");
            }

            testCase.Key = keyResult.KeyPair;

            // Generate the signature
            FfcSignatureResult sigResult = null;
            try
            {
                sigResult = _ffcDsa.Sign(group.DomainParams, testCase.Key, testCase.Message);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature: {sigResult.ErrorMessage}");
                return new TestCaseGenerateResponse($"Exception generating signature: {sigResult.ErrorMessage}");
            }

            testCase.DomainParams = group.DomainParams;
            testCase.Signature = sigResult.Signature;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

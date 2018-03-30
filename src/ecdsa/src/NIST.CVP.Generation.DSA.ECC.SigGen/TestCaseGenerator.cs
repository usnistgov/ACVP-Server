using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.ECC.SigGen
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random;
        private readonly IDsaEcc _eccDsa;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGenerator(IRandom800_90 rand, IDsaEcc eccDsa)
        {
            _random = rand;
            _eccDsa = eccDsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            // For component test, get a post-hash message (just random value of hash output length)
            var testCase = new TestCase
            {
                Message = _random.GetRandomBitString(group.ComponentTest ? group.HashAlg.OutputLen : 1024)
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
            // Generate the key
            EccKeyPairGenerateResult keyResult = null;
            try
            {
                keyResult = _eccDsa.GenerateKeyPair(group.DomainParameters);
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

            testCase.KeyPair = keyResult.KeyPair;

            // Generate the signature
            EccSignatureResult sigResult = null;
            try
            {
                sigResult = _eccDsa.Sign(group.DomainParameters, testCase.KeyPair, testCase.Message, group.ComponentTest);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature: {sigResult.ErrorMessage}, {ex.Message}");
                return new TestCaseGenerateResponse($"Exception generating signature: {sigResult.ErrorMessage}");
            }

            testCase.Signature = sigResult.Signature;
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

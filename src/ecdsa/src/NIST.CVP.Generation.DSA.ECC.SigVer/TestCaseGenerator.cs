using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.DSA.ECC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.ECC.SigVer.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.ECC.SigVer
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; private set; } = 15;
        private readonly IRandom800_90 _rand;
        private readonly IDsaEcc _eccDsa;

        public TestCaseGenerator(IRandom800_90 rand, IDsaEcc eccDsa)
        {
            _rand = rand;
            _eccDsa = eccDsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var keyResult = _eccDsa.GenerateKeyPair(group.DomainParameters);
            if (!keyResult.Success)
            {
                return new TestCaseGenerateResponse(keyResult.ErrorMessage);
            }

            var reason = group.TestCaseExpectationProvider.GetRandomReason();

            var testCase = new TestCase()
            {
                Message = _rand.GetRandomBitString(1024),
                KeyPair = keyResult.KeyPair,
                Reason = reason.GetReason(),
                FailureTest = (reason.GetReason() != SigFailureReasons.None),
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            EccSignatureResult sigResult = null;
            try
            {
                sigResult = _eccDsa.Sign(group.DomainParameters, testCase.KeyPair, testCase.Message);
                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating g: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating g: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating g: {ex.StackTrace}");
                return new TestCaseGenerateResponse($"Exception generating g: {ex.StackTrace}");
            }

            testCase.Signature = sigResult.Signature;

            return new TestCaseGenerateResponse(ModifyTestCase(testCase, group.DomainParameters));
        }

        private TestCase ModifyTestCase(TestCase testCase, EccDomainParameters domainParams)
        {
            var modifiedTestCase = testCase;

            if (testCase.Reason == SigFailureReasons.ModifyMessage)
            {
                // Generate a different random message
                modifiedTestCase.Message = _rand.GetDifferentBitStringOfSameSize(testCase.Message);
            }
            else if (testCase.Reason == SigFailureReasons.ModifyKey)
            {
                // Generate a different key pair for the test case
                var keyResult = _eccDsa.GenerateKeyPair(domainParams);
                modifiedTestCase.KeyPair = keyResult.KeyPair;
            }
            else if (testCase.Reason == SigFailureReasons.ModifyR)
            {
                var modifiedRSignature = new EccSignature(testCase.Signature.R + 1, testCase.Signature.S);
                modifiedTestCase.Signature = modifiedRSignature;
            }
            else if (testCase.Reason == SigFailureReasons.ModifyS)
            {
                var modifiedSSignature = new EccSignature(testCase.Signature.R, testCase.Signature.S + 1);
                modifiedTestCase.Signature = modifiedSSignature;
            }

            return modifiedTestCase;
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

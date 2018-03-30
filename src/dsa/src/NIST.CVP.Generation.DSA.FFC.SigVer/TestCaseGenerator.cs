using System;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Crypto.DSA.FFC;
using NIST.CVP.Generation.Core;
using NIST.CVP.Generation.DSA.FFC.SigVer.Enums;
using NIST.CVP.Math;
using NLog;

namespace NIST.CVP.Generation.DSA.FFC.SigVer
{
    public class TestCaseGenerator : ITestCaseGenerator<TestGroup, TestCase>
    {
        public int NumberOfTestCasesToGenerate { get; private set; } = 15;
        private readonly IRandom800_90 _rand;
        private readonly IDsaFfc _ffcDsa;

        public TestCaseGenerator(IRandom800_90 rand, IDsaFfc ffcDsa)
        {
            _rand = rand;
            _ffcDsa = ffcDsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 5;
            }

            var keyResult = _ffcDsa.GenerateKeyPair(group.DomainParams);
            if (!keyResult.Success)
            {
                return new TestCaseGenerateResponse(keyResult.ErrorMessage);
            }

            var reason = group.TestCaseExpectationProvider.GetRandomReason();

            var testCase = new TestCase()
            {
                Message = _rand.GetRandomBitString(group.N),
                Key = keyResult.KeyPair,
                Reason = reason,
                FailureTest = (reason.GetReason() != SigFailureReasons.None),
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            FfcSignatureResult sigResult = null;
            try
            {
                sigResult = _ffcDsa.Sign(group.DomainParams, testCase.Key, testCase.Message);
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

            // Modify message
            //var modifiedTestBuilder = new ModifiedTestCaseBuilder();
            if (testCase.Reason.GetReason() == SigFailureReasons.ModifyMessage)
            {
                //testCase = modifiedTestBuilder.WithTestCase(testCase).Apply(modifiedTestBuilder.ModifyMessage).Build();
                testCase.Message = _rand.GetDifferentBitStringOfSameSize(testCase.Message);
            }
            // Modify public key
            else if (testCase.Reason.GetReason() == SigFailureReasons.ModifyKey)
            {
                var x = testCase.Key.PrivateKeyX;
                var y = testCase.Key.PublicKeyY + 2;
                testCase.Key = new FfcKeyPair(x, y);
            }
            // Modify r
            else if (testCase.Reason.GetReason() == SigFailureReasons.ModifyR)
            {
                var s = testCase.Signature.S;
                var r = testCase.Signature.R + 2;
                testCase.Signature = new FfcSignature(s, r);
            }
            // Modify s
            else if (testCase.Reason.GetReason() == SigFailureReasons.ModifyS)
            {
                var s = testCase.Signature.S + 2;
                var r = testCase.Signature.R;
                testCase.Signature = new FfcSignature(s, r);
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

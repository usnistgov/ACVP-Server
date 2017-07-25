using NIST.CVP.Crypto.RSA;
using NIST.CVP.Crypto.RSA.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly SignerBase _signer;

        public int NumberOfTestCasesToGenerate { get { return 10; } }

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, SignerBase signer)
        {
            _random800_90 = random800_90;
            _signer = signer;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
                IsSample = isSample
            };

            if (group.Mode == SigGenModes.PSS)
            {
                if (group.SaltMode == SaltModes.RANDOM)
                {
                    testCase.Salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                }
                else if (group.SaltMode == SaltModes.FIXED)
                {
                    testCase.Salt = group.Salt;
                }
            }

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            if (testCase.IsSample)
            {
                SignatureResult sigResult = null;
                try
                {
                    _signer.SetHashFunction(group.HashAlg);

                    if (group.Mode == SigGenModes.PSS)
                    {
                        _signer.AddEntropy(testCase.Salt);
                    }

                    sigResult = _signer.Sign(group.Modulo, testCase.Message, group.Key);
                    if (!sigResult.Success)
                    {
                        ThisLogger.Warn($"Error generating sample signature: {sigResult.ErrorMessage}");
                        return new TestCaseGenerateResponse($"Error generating sample signature: {sigResult.ErrorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    ThisLogger.Error($"Exception generating sample signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Exception generating sample signature: {sigResult.ErrorMessage}");
                }

                testCase.Signature = sigResult.Signature;
            }

            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger { get { return LogManager.GetCurrentClassLogger(); } }
    }
}

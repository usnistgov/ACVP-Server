using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;

        public int NumberOfTestCasesToGenerate { get; private set; } = 10;

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, ISignatureBuilder signatureBuilder, IPaddingFactory paddingFactory, IShaFactory shaFactory, IRsa rsa)
        {
            _random800_90 = random800_90;
            _signatureBuilder = signatureBuilder;
            _paddingFactory = paddingFactory;
            _shaFactory = shaFactory;
            _rsa = rsa;
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, bool isSample)
        {
            if (isSample)
            {
                NumberOfTestCasesToGenerate = 3;
            }

            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
            };

            return isSample ? Generate(group, testCase) : new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        public TestCaseGenerateResponse<TestGroup, TestCase> Generate(TestGroup group, TestCase testCase)
        {
            SignatureResult sigResult = null;
            try
            {
                var entropyProvider = new TestableEntropyProvider();
                testCase.Salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                entropyProvider.AddEntropy(testCase.Salt);

                var sha = _shaFactory.GetShaInstance(group.HashAlg);

                var paddingScheme = _paddingFactory.GetPaddingScheme(group.Mode, sha, entropyProvider, group.SaltLen);

                sigResult = _signatureBuilder
                    .WithKey(group.Key)
                    .WithMessage(testCase.Message)
                    .WithPaddingScheme(paddingScheme)
                    .WithDecryptionScheme(_rsa)
                    .BuildSign();

                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating sample signature: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse<TestGroup, TestCase>($"Error generating sample signature: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Error($"Exception generating sample signature: {ex.StackTrace}");
                return new TestCaseGenerateResponse<TestGroup, TestCase>($"Exception generating sample signature: {ex.StackTrace}");
            }

            testCase.Signature = new BitString(sigResult.Signature, group.Modulo);

            return new TestCaseGenerateResponse<TestGroup, TestCase>(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

using NIST.CVP.Generation.Core;
using NIST.CVP.Math;
using NLog;
using System;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Enums;
using NIST.CVP.Crypto.Common.Asymmetric.RSA2.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigVer
{
    public class TestCaseGeneratorGDT : ITestCaseGenerator<TestGroup, TestCase>
    {
        private readonly IRandom800_90 _random800_90;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IPaddingFactory _paddingFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;

        public int NumberOfTestCasesToGenerate { get; private set; } = 6;

        public TestCaseGeneratorGDT(IRandom800_90 random800_90, ISignatureBuilder sigBuilder, IPaddingFactory padFactory, IShaFactory shaFactory, IRsa rsa)
        {
            _random800_90 = random800_90;
            _signatureBuilder = sigBuilder;
            _paddingFactory = padFactory;
            _shaFactory = shaFactory;
            _rsa = rsa;
        }

        public TestCaseGenerateResponse Generate(TestGroup group, bool isSample)
        {
            var reason = group.TestCaseExpectationProvider.GetRandomReason();

            var testCase = new TestCase
            {
                Message = _random800_90.GetRandomBitString(group.Modulo / 2),
                Reason = reason,
                FailureTest = reason.GetReason() != SignatureModifications.None
            };

            return Generate(group, testCase);
        }

        public TestCaseGenerateResponse Generate(TestGroup group, TestCase testCase)
        {
            SignatureResult sigResult = null;
            BitString salt;
            try
            {
                var sha = _shaFactory.GetShaInstance(group.HashAlg);
                salt = _random800_90.GetRandomBitString(group.SaltLen * 8);
                var entropyProvider = new TestableEntropyProvider();
                entropyProvider.AddEntropy(salt);

                var paddingScheme = _paddingFactory.GetSigningPaddingScheme(group.Mode, sha, testCase.Reason.GetReason(), entropyProvider, group.SaltLen);

                sigResult = _signatureBuilder
                    .WithDecryptionScheme(_rsa)
                    .WithMessage(testCase.Message)
                    .WithPaddingScheme(paddingScheme)
                    .WithKey(group.Key)
                    .BuildSign();

                if (!sigResult.Success)
                {
                    ThisLogger.Warn($"Error generating signature with intentional errors: {sigResult.ErrorMessage}");
                    return new TestCaseGenerateResponse($"Error generating signature with intentional errors: {sigResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                ThisLogger.Warn($"Exception generating signature with intentional errors: {ex.Source}");
                return new TestCaseGenerateResponse($"Exception generating signature with intentional errors: {ex.Source}");
            }

            if (group.Mode == SignatureSchemes.Pss)
            {
                testCase.Salt = salt;
            }

            testCase.Signature = new BitString(sigResult.Signature);
            return new TestCaseGenerateResponse(testCase);
        }

        private Logger ThisLogger => LogManager.GetCurrentClassLogger();
    }
}

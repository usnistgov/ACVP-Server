using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.RSA2;
using NIST.CVP.Crypto.RSA2.Signatures;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult>
    {
        private readonly IPaddingFactory _paddingFactory;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IShaFactory _shaFactory;

        public DeferredTestCaseResolver(IPaddingFactory paddingFactory, ISignatureBuilder sigBuilder, IShaFactory shaFactory)
        {
            _paddingFactory = paddingFactory;
            _signatureBuilder = sigBuilder;
            _shaFactory = shaFactory;
        }

        public VerifyResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var sha = _shaFactory.GetShaInstance(testGroup.HashAlg);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(serverTestCase.Salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(testGroup.Mode, sha, entropyProvider, testGroup.SaltLen);

            return _signatureBuilder
                .WithDecryptionScheme(new Rsa(new RsaVisitor()))
                .WithKey(iutTestCase.Key)
                .WithMessage(serverTestCase.Message)
                .WithPaddingScheme(paddingScheme)
                .WithSignature(iutTestCase.Signature)
                .BuildVerify();
        }
    }
}

using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Signatures;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Generation.Core;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Generation.RSA_SigGen
{
    public class DeferredTestCaseResolver : IDeferredTestCaseResolver<TestGroup, TestCase, VerifyResult>
    {
        private readonly IPaddingFactory _paddingFactory;
        private readonly ISignatureBuilder _signatureBuilder;
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;

        public DeferredTestCaseResolver(IPaddingFactory paddingFactory, ISignatureBuilder sigBuilder, IShaFactory shaFactory, IRsa rsa)
        {
            _paddingFactory = paddingFactory;
            _signatureBuilder = sigBuilder;
            _shaFactory = shaFactory;
            _rsa = rsa;
        }

        public VerifyResult CompleteDeferredCrypto(TestGroup serverTestGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var iutTestGroup = iutTestCase.ParentGroup;
            var sha = _shaFactory.GetShaInstance(serverTestGroup.HashAlg);

            var entropyProvider = new TestableEntropyProvider();
            entropyProvider.AddEntropy(serverTestCase.Salt);

            var paddingScheme = _paddingFactory.GetPaddingScheme(serverTestGroup.Mode, sha, entropyProvider, serverTestGroup.SaltLen);

            return _signatureBuilder
                .WithDecryptionScheme(_rsa)
                .WithKey(iutTestGroup.Key)
                .WithMessage(serverTestCase.Message)
                .WithPaddingScheme(paddingScheme)
                .WithSignature(iutTestCase.Signature)
                .BuildVerify();
        }
    }
}

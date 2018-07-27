using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_GCM
{
    public class DeferredEncryptResolver : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCipherAeadResult>
    {
        private readonly IAeadModeBlockCipher _algo;

        public DeferredEncryptResolver(IAeadModeBlockCipher algo)
        {
            _algo = algo;
        }

        public SymmetricCipherAeadResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                iutTestCase.IV,
                serverTestCase.Key,
                serverTestCase.PlainText,
                serverTestCase.AAD,
                testGroup.TagLength
            );

            return _algo.ProcessPayload(param);
        }
    }
}
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly ICounterModeBlockCipher _algo;

        public DeferredTestCaseResolverEncrypt(ICounterModeBlockCipher algo)
        {
            _algo = algo;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new CounterModeBlockCipherParameters(BlockCipherDirections.Encrypt, serverTestCase.Key, serverTestCase.PlainText, iutTestCase.CipherText);
            return _algo.ExtractIvs(param);
        }
    }
}

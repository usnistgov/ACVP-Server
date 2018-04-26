using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Generation.Core;
using Cipher = NIST.CVP.Crypto.Common.Symmetric.CTR.Enums.Cipher;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverEncrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(Cipher.AES, iutTestCase.IVs);
            return _algo.Encrypt(serverTestCase.Key, serverTestCase.PlainText, counter);
        }
    }
}

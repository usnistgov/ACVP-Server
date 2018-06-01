using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.AES_CTR
{
    public class DeferredTestCaseResolverDecrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IAesCtr _algo;

        public DeferredTestCaseResolverDecrypt(IAesCtr algo)
        {
            _algo = algo;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            return _algo.CounterDecrypt(serverTestCase.Key, serverTestCase.CipherText, serverTestCase.PlainText);
        }
    }
}

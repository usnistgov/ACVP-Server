using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class DeferredTestCaseResolverDecrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly ICounterModeBlockCipher _algo;

        public DeferredTestCaseResolverDecrypt(ICounterModeBlockCipher algo)
        {
            _algo = algo;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var param = new CounterModeBlockCipherParameters(BlockCipherDirections.Decrypt, serverTestCase.Key, serverTestCase.CipherText, iutTestCase.PlainText);
            return _algo.ExtractIvs(param);
        }
    }
}

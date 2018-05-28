using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.CTR.Fakes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Generation.Core;

namespace NIST.CVP.Generation.TDES_CTR
{
    public class DeferredTestCaseResolverEncrypt : IDeferredTestCaseResolver<TestGroup, TestCase, SymmetricCounterResult>
    {
        private readonly IBlockCipherEngine _engine;
        private readonly IModeBlockCipherFactory _modeFactory;

        public DeferredTestCaseResolverEncrypt(
            IBlockCipherEngineFactory engineFactory, 
            IModeBlockCipherFactory modeFactory
        )
        {
            _engine = engineFactory.GetSymmetricCipherPrimitive(BlockCipherEngines.Tdes);
            _modeFactory = modeFactory;
        }

        public SymmetricCounterResult CompleteDeferredCrypto(TestGroup testGroup, TestCase serverTestCase, TestCase iutTestCase)
        {
            var counter = new TestableCounter(_engine, iutTestCase.Ivs);
            var algo = _modeFactory.GetCounterCipher(_engine, counter);
            return algo.ProcessPayload(new ModeBlockCipherParameters(
                BlockCipherDirections.Encrypt, serverTestCase.Key, serverTestCase.PlainText
            ));
        }
    }
}

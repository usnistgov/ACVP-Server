using Moq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DrbgCounterAesTests
    {
        private Mock<IBlockCipherEngineFactory> _engineFactory = new Mock<IBlockCipherEngineFactory>();
        private Mock<IModeBlockCipherFactory> _cipherFactory = new Mock<IModeBlockCipherFactory>();
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _engine = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
        private FakeDrbgCounterAes _subject;

        [SetUp]
        public void Setup()
        {
            _engine
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult(new BitString(0)));

            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(new AesEngine());

            _cipherFactory
                .Setup(s => s.GetStandardCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(_engine.Object);

            _subject = new FakeDrbgCounterAes(new Mock<IEntropyProvider>().Object, _engineFactory.Object, _cipherFactory.Object, new DrbgParameters());
        }

        [Test]
        public void ShouldCallUnderlyingAesEncrypt()
        {
            var k = new BitString(10);
            var x = new BitString(100);

            _subject.PublicBlockEncrypt(k, x);

            _engine.Verify(v => v.ProcessPayload(It.IsAny<ModeBlockCipherParameters>()), Times.Once);
        }
    }
}

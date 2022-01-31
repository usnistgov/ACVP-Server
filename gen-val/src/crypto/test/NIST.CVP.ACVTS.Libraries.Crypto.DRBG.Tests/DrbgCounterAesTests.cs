using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.Tests
{
    [TestFixture, FastCryptoTest]
    public class DrbgCounterAesTests
    {
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

        private Mock<IBlockCipherEngineFactory> _engineFactory = new Mock<IBlockCipherEngineFactory>();
        private Mock<IModeBlockCipherFactory> _cipherFactory = new Mock<IModeBlockCipherFactory>();
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _engine = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
        private FakeDrbgCounterAes _subject;

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

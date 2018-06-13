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
    [TestFixture, FastCryptoTest]
    public class DrbgCounterTdesTests
    {
        private Mock<IBlockCipherEngineFactory> _engineFactory = new Mock<IBlockCipherEngineFactory>();
        private Mock<IModeBlockCipherFactory> _cipherFactory = new Mock<IModeBlockCipherFactory>();
        private Mock<IModeBlockCipher<SymmetricCipherResult>> _cipher = new Mock<IModeBlockCipher<SymmetricCipherResult>>();
        private FakeDrbgCounterTdes _subject;

        [SetUp]
        public void Setup()
        {
            _cipher
                .Setup(s => s.ProcessPayload(It.IsAny<IModeBlockCipherParameters>()))
                .Returns(new SymmetricCipherResult(new BitString(0)));

            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(new TdesEngine());

            _cipherFactory
                .Setup(s => s.GetStandardCipher(It.IsAny<IBlockCipherEngine>(), It.IsAny<BlockCipherModesOfOperation>()))
                .Returns(_cipher.Object);

            _subject = new FakeDrbgCounterTdes(new Mock<IEntropyProvider>().Object, _engineFactory.Object, _cipherFactory.Object, new DrbgParameters());
        }

        [Test]
        public void ShouldCallUnderlyingTdesEncrypt()
        {
            var k = new BitString(168);
            var x = new BitString(100);

            _subject.PublicBlockEncrypt(k, x);

            _cipher.Verify(v => v.ProcessPayload(It.IsAny<ModeBlockCipherParameters>()), Times.Once);
        }

        [Test]
        [TestCase("000000000000000000000000000000000000000000", "000000000000000000000000000000000000000000000000")]
        [TestCase("166a42b74ebf4dd10eeee029c29f7e805ea082dab6", "163490567474fc9ad086badc024e0a3e7e4016d408166a6c")]
        [TestCase("eab6f0c72b1c15a6ee7244fcab83af9d17590cc9c2", "ea5abc187258702aa6769c484ee4ae06aece44ea90662684")]
        public void ShouldCorrectlyConvert168BitKeyTo192BitKey(string keyHex, string expectedHex)
        {
            var k = new BitString(keyHex);
            var expected = new BitString(expectedHex);

            var result = _subject.Convert168BitKeyTo192BitKey_public(k);

            var expectedArr = new BitString[3];
            expectedArr[0] = expected.MSBSubstring(0, 64);
            expectedArr[1] = expected.MSBSubstring(64, 64);
            expectedArr[2] = expected.MSBSubstring(64 * 2, 64);

            var resultArr = new BitString[3];
            resultArr[0] = result.MSBSubstring(0, 64);
            resultArr[1] = result.MSBSubstring(64, 64);
            resultArr[2] = result.MSBSubstring(64 * 2, 64);

            for (var i = 0; i < 3; i++)
            {
                Assert.AreEqual(expectedArr[i], resultArr[i], $"{i}");
            }

            Assert.AreEqual(expected, result, "whole");
        }
    }
}

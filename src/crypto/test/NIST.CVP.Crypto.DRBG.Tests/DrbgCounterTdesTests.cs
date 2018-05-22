using Moq;
using NIST.CVP.Crypto.Common.DRBG;
using NIST.CVP.Crypto.Common.Symmetric;
using NIST.CVP.Crypto.Common.Symmetric.TDES;
using NIST.CVP.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.Crypto.TDES;
using NIST.CVP.Crypto.TDES_ECB;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture, FastCryptoTest]
    public class DrbgCounterTdesTests
    {
        private Mock<ITDES_ECB> _mockTdes;
        private FakeDrbgCounterTdes _subject;

        [SetUp]
        public void Setup()
        {
            _mockTdes = new Mock<ITDES_ECB>();
            _subject = new FakeDrbgCounterTdes(new Mock<IEntropyProvider>().Object, _mockTdes.Object, new DrbgParameters());
        }

        [Test]
        public void ShouldCallUnderlyingTdesEncrypt()
        {
            // Needs to be 168 because underlying call that happens before blockEncrypt needs it
            BitString k = new BitString(168);
            BitString x = new BitString(100);

            _mockTdes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new SymmetricCipherResult(new BitString(0)));

            _subject.PublicBlockEncrypt(k, x);

            _mockTdes.Verify(v => v.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false), Times.Once);
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

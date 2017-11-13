using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using NIST.CVP.Crypto.AES_ECB;
using NIST.CVP.Crypto.DRBG.Tests.Fakes;
using NIST.CVP.Math;
using NIST.CVP.Math.Entropy;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.DRBG.Tests
{
    [TestFixture,  FastCryptoTest]
    public class DrbgCounterAesTests
    {
        private Mock<IAES_ECB> _mockAes;
        private FakeDrbgCounterAes _subject;

        [SetUp]
        public void Setup()
        {
            _mockAes = new Mock<IAES_ECB>();
            _subject = new FakeDrbgCounterAes(new Mock<IEntropyProvider>().Object, _mockAes.Object, new DrbgParameters(), 0);
        }

        [Test]
        public void ShouldCallUnderlyingAesEncrypt()
        {
            BitString k = new BitString(10);
            BitString x = new BitString(100);

            _mockAes
                .Setup(s => s.BlockEncrypt(It.IsAny<BitString>(), It.IsAny<BitString>(), false))
                .Returns(new EncryptionResult(new BitString(0)));

            _subject.PublicBlockEncrypt(k, x);

            _mockAes.Verify(v => v.BlockEncrypt(k, x, false), Times.Once);
        }
    }
}

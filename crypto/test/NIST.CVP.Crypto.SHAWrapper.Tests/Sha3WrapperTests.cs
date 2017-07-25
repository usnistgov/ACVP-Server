using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.SHA3;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHAWrapper.Tests
{
    [TestFixture, UnitTest]
    public class Sha3WrapperTests
    {
        private Mock<ISHA3Factory> _shaFactory;
        private Mock<SHA3Wrapper> _shaWrapper;
        private HashFunction _hashFunction;
        private Sha3Wrapper _subject;

        [SetUp]
        public void Setup()
        {
            _shaFactory = new Mock<ISHA3Factory>();
            _shaWrapper = new Mock<SHA3Wrapper>();
            _hashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d224);

            _shaFactory
                .Setup(s => s.GetSHA(It.IsAny<SHA3.HashFunction>()))
                .Returns(_shaWrapper.Object);
            _shaWrapper
                .Setup(s => s.HashMessage(It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<int>(), false))
                .Returns(new BitString(0));

            _subject = new Sha3Wrapper(_shaFactory.Object, _hashFunction);
        }

        [Test]
        public void ShouldCallUnderlyingFactoryMethod()
        {
            _subject.HashMessage(It.IsAny<BitString>());

            _shaFactory.Verify(v => v.GetSHA(It.IsAny<SHA3.HashFunction>()), 
                Times.Once(), 
                nameof(_shaFactory.Object.GetSHA)
            );
        }

        [Test]
        public void ShouldCallUnderlyingShaHashMessage()
        {
            _subject.HashMessage(It.IsAny<BitString>());

            _shaWrapper.Verify(v => v.HashMessage(It.IsAny<BitString>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>()), 
                Times.Once, 
                nameof(_shaWrapper.Object.HashMessage)
            );
        }

        [Test]
        [TestCase(ModeValues.SHA3, DigestSizes.d224)]
        [TestCase(ModeValues.SHA3, DigestSizes.d256)]
        [TestCase(ModeValues.SHA3, DigestSizes.d384)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512)]
        public void ShouldReturnCorrectModeAndSize(ModeValues mode, DigestSizes digestSize)
        {
            HashFunction hashFunction = new HashFunction(mode, digestSize);

            _subject = new Sha3Wrapper(_shaFactory.Object, hashFunction);

            Assert.AreEqual(mode, _subject.HashFunction.Mode, nameof(mode));
            Assert.AreEqual(digestSize, _subject.HashFunction.DigestSize, nameof(digestSize));
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d256)]
        [TestCase(ModeValues.SHA1, DigestSizes.d384)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA1, DigestSizes.d512t256)]
        [TestCase(ModeValues.SHA2, DigestSizes.d160)]
        [TestCase(ModeValues.SHA3, DigestSizes.d160)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512t224)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512t256)]
        public void ShouldThrowWithInvalidModeDigestCombination(ModeValues mode, DigestSizes digestSize)
        {
            HashFunction hashFunction = new HashFunction(mode, digestSize);

            Assert.Throws(typeof(ArgumentException), () => _subject = new Sha3Wrapper(_shaFactory.Object, hashFunction));
        }
    }
}

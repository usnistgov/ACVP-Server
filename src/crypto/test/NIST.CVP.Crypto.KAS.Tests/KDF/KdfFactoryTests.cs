using System;
using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KDF
{
    [TestFixture,  FastCryptoTest]
    public class KdfFactoryTests
    {
        private KdfFactory _subject;
        private Mock<IShaFactory> _shaFactory;
        private Mock<ISha> _sha;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();

            _shaFactory = new Mock<IShaFactory>();
            _shaFactory
                .Setup(s => s.GetShaInstance(It.IsAny<HashFunction>()))
                .Returns(_sha.Object);

            _subject = new KdfFactory(_shaFactory.Object);
        }

        [Test]
        [TestCase(KdfHashMode.Sha, typeof(KdfSha))]
        public void ShouldReturnCorrectImplementation(KdfHashMode kdfHashMode, Type expectedType)
        {
            var result = _subject.GetInstance(kdfHashMode, new HashFunction(0, 0));

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidEnum()
        {
            int i = -1;
            var badType = (KdfHashMode)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(badType, new HashFunction(0, 0)));
        }
    }
}
using System;
using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.HMAC;
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
        private Mock<IHmacFactory> _hmacFactory;
        private Mock<ISha> _sha;
        private Mock<Hmac> _hmac;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _hmac = new Mock<Hmac>();

            _shaFactory = new Mock<IShaFactory>();
            _shaFactory
                .Setup(s => s.GetShaInstance(It.IsAny<HashFunction>()))
                .Returns(_sha.Object);
            
            _hmacFactory = new Mock<IHmacFactory>();
            _hmacFactory
                .Setup(s => s.GetHmacInstance(It.IsAny<HashFunction>()))
                .Returns(_hmac.Object);

            _subject = new KdfFactory(_shaFactory.Object, _hmacFactory.Object);
        }

        [Test]
        [TestCase(KdfHashMode.Sha, typeof(KdfSha))]
        public void ShouldReturnCorrectImplementation(KdfHashMode kdfHashMode, Type expectedType)
        {
            var result = _subject.GetInstance(kdfHashMode, new HashFunction(ModeValues.SHA1, DigestSizes.d160));

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidEnum()
        {
            int i = -1;
            var badType = (KdfHashMode)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(badType, new HashFunction(ModeValues.SHA1, DigestSizes.d160)));
        }
    }
}
using System;
using Moq;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.KDF;
using NIST.CVP.Crypto.KAS.KDF.OneStep;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class KdfFactoryTests
    {
        private KdfOneStepFactory _subject;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IHmacFactory> _hmacFactory;
        private Mock<ISha> _sha;
        private Mock<IHmac> _hmac;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _hmac = new Mock<IHmac>();

            _shaFactory = new Mock<IShaFactory>();
            _shaFactory
                .Setup(s => s.GetShaInstance(It.IsAny<HashFunction>()))
                .Returns(_sha.Object);
            
            _hmacFactory = new Mock<IHmacFactory>();
            _hmacFactory
                .Setup(s => s.GetHmacInstance(It.IsAny<HashFunction>()))
                .Returns(_hmac.Object);

            _subject = new KdfOneStepFactory(_shaFactory.Object, _hmacFactory.Object);
        }

        [Test]
        [TestCase(KasKdfOneStepAuxFunction.SHA2_D224, typeof(KdfSha))]
        [TestCase(KasKdfOneStepAuxFunction.HMAC_SHA2_D224, typeof(KdfHmac))]
        public void ShouldReturnCorrectImplementation(KasKdfOneStepAuxFunction auxFunction, Type expectedType)
        {
            var result = _subject.GetInstance(auxFunction);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidEnum()
        {
            int i = -1;
            var badType = (KasKdfOneStepAuxFunction)i;
            
            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(badType));
        }
    }
}
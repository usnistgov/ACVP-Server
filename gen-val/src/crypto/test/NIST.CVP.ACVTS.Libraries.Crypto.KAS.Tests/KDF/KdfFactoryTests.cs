using System;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDA.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.Tests.KDF
{
    [TestFixture, FastCryptoTest]
    public class KdfFactoryTests
    {
        private KdfOneStepFactory _subject;
        private Mock<IShaFactory> _shaFactory;
        private Mock<IHmacFactory> _hmacFactory;
        private Mock<IKmacFactory> _kmacFactory;
        private Mock<ISha> _sha;
        private Mock<IHmac> _hmac;
        private Mock<IKmac> _kmac;

        [SetUp]
        public void Setup()
        {
            _sha = new Mock<ISha>();
            _hmac = new Mock<IHmac>();
            _kmac = new Mock<IKmac>();

            _shaFactory = new Mock<IShaFactory>();
            _shaFactory
                .Setup(s => s.GetShaInstance(It.IsAny<HashFunction>()))
                .Returns(_sha.Object);

            _hmacFactory = new Mock<IHmacFactory>();
            _hmacFactory
                .Setup(s => s.GetHmacInstance(It.IsAny<HashFunction>()))
                .Returns(_hmac.Object);

            _kmacFactory = new Mock<IKmacFactory>();
            _kmacFactory
                .Setup(s => s.GetKmacInstance(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(_kmac.Object);

            _subject = new KdfOneStepFactory(_shaFactory.Object, _hmacFactory.Object, _kmacFactory.Object);
        }

        [Test]
        [TestCase(KdaOneStepAuxFunction.SHA2_D224, typeof(KdfSha))]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D224, typeof(KdfHmac))]
        public void ShouldReturnCorrectImplementation(KdaOneStepAuxFunction auxFunction, Type expectedType)
        {
            var result = _subject.GetInstance(auxFunction, true);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidEnum()
        {
            int i = -1;
            var badType = (KdaOneStepAuxFunction)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetInstance(badType, true));
        }

        [Test]
        [TestCase(KdaOneStepAuxFunction.SHA1, ModeValues.SHA1, DigestSizes.d160)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D224, ModeValues.SHA2, DigestSizes.d224)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D256, ModeValues.SHA2, DigestSizes.d256)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D384, ModeValues.SHA2, DigestSizes.d384)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512, ModeValues.SHA2, DigestSizes.d512)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T224, ModeValues.SHA2, DigestSizes.d512t224)]
        [TestCase(KdaOneStepAuxFunction.SHA2_D512_T256, ModeValues.SHA2, DigestSizes.d512t256)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D224, ModeValues.SHA3, DigestSizes.d224)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D256, ModeValues.SHA3, DigestSizes.d256)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D384, ModeValues.SHA3, DigestSizes.d384)]
        [TestCase(KdaOneStepAuxFunction.SHA3_D512, ModeValues.SHA3, DigestSizes.d512)]
        public void ShaShouldReturnProperHashFunctionFromConstruction(KdaOneStepAuxFunction kdfOneStepAuxFunction, ModeValues mode, DigestSizes digestSize)
        {
            _subject.GetInstance(kdfOneStepAuxFunction, true);

            _shaFactory.Verify(v => v.GetShaInstance(new HashFunction(mode, digestSize, false)));
        }

        [Test]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA1, ModeValues.SHA1, DigestSizes.d160)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D224, ModeValues.SHA2, DigestSizes.d224)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D256, ModeValues.SHA2, DigestSizes.d256)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D384, ModeValues.SHA2, DigestSizes.d384)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512, ModeValues.SHA2, DigestSizes.d512)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T224, ModeValues.SHA2, DigestSizes.d512t224)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA2_D512_T256, ModeValues.SHA2, DigestSizes.d512t256)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D224, ModeValues.SHA3, DigestSizes.d224)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D256, ModeValues.SHA3, DigestSizes.d256)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D384, ModeValues.SHA3, DigestSizes.d384)]
        [TestCase(KdaOneStepAuxFunction.HMAC_SHA3_D512, ModeValues.SHA3, DigestSizes.d512)]
        public void HmacShouldReturnProperHashFunctionFromConstruction(KdaOneStepAuxFunction kdfOneStepAuxFunction, ModeValues mode, DigestSizes digestSize)
        {
            _subject.GetInstance(kdfOneStepAuxFunction, true);

            _shaFactory.Verify(v => v.GetShaInstance(new HashFunction(mode, digestSize, false)));
        }
    }
}

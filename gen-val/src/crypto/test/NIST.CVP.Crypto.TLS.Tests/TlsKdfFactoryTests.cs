using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.TLS.Tests
{
    [TestFixture, FastCryptoTest]
    public class TlsKdfFactoryTests
    {
        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, TlsModes.v10v11, typeof(TlsKdfv10v11))]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, TlsModes.v12, typeof(TlsKdfv12))]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, TlsModes.v12, typeof(TlsKdfv12))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, TlsModes.v12, typeof(TlsKdfv12))]
        public void ShouldProvideCorrectTlsKdfInstance(ModeValues mode, DigestSizes digestSize, TlsModes tlsMode, Type expectedType)
        {
            var hashFunction = new HashFunction(mode, digestSize);
            var subject = new TlsKdfFactory();
            var result = subject.GetTlsKdfInstance(tlsMode, hashFunction);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, TlsModes.v12)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, TlsModes.v10v11)]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, TlsModes.v12)]
        public void ShouldExceptionWithBadHashFunction(ModeValues mode, DigestSizes digestSize, TlsModes tlsMode)
        {
            var hashFunction = new HashFunction(mode, digestSize);
            var subject = new TlsKdfFactory();

            Assert.Throws(typeof(ArgumentException), () => subject.GetTlsKdfInstance(tlsMode, hashFunction));
        }
    }
}


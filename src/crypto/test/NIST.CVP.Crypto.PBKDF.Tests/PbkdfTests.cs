using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.PBKDF.Tests
{
    [TestFixture, LongCryptoTest]
    public class PbkdfTests
    {
        private PbKdfFactory _factory;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _factory = new PbKdfFactory(new ShaFactory(), new FastHmacFactory(new ShaFactory()));
        }

        [Test]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, 1000000)]
        
        [TestCase(ModeValues.SHA2, DigestSizes.d224, 1000)]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, 1000000)]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, 1000000)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, 1000000)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, 1000)]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, 1000)]
        
        [TestCase(ModeValues.SHA3, DigestSizes.d224, 1000)]
        [TestCase(ModeValues.SHA3, DigestSizes.d256, 1000)]
        [TestCase(ModeValues.SHA3, DigestSizes.d384, 1000)]
        [TestCase(ModeValues.SHA3, DigestSizes.d512, 1000)]
        public void LargeTest(ModeValues mode, DigestSizes digest, int c)
        {
            var pbkdf = _factory.GetKdf(new HashFunction(mode, digest));
            var result = pbkdf.DeriveKey(new BitString("ABDC"), "abcd", c, 1024);

            Console.Write(result.DerivedKey.ToHex());
            Assert.Pass();
        }
    }
}
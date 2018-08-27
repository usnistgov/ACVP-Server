using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.SHAWrapper.Tests
{
    [TestFixture,  FastCryptoTest]
    public class ShaFactoryTests
    {

        private ShaFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new ShaFactory();
        }

        [Test, FastCryptoTest]
        [TestCase(ModeValues.SHA1, DigestSizes.d160, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d224, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d256, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d384, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t224, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA2, DigestSizes.d512t256, typeof(Sha2Wrapper))]
        [TestCase(ModeValues.SHA3, DigestSizes.d224, typeof(Sha3Wrapper))]
        [TestCase(ModeValues.SHA3, DigestSizes.d256, typeof(Sha3Wrapper))]
        [TestCase(ModeValues.SHA3, DigestSizes.d384, typeof(Sha3Wrapper))]
        [TestCase(ModeValues.SHA3, DigestSizes.d512, typeof(Sha3Wrapper))]
        [TestCase(ModeValues.SHAKE, DigestSizes.d128, typeof(ShakeWrapper))]
        [TestCase(ModeValues.SHAKE, DigestSizes.d256, typeof(ShakeWrapper))]
        public void ShouldReturnCorrectInstance(ModeValues mode, DigestSizes digestSize, Type expectedType)
        {
            HashFunction hashFunction = new HashFunction(mode, digestSize);

            var result = _subject.GetShaInstance(hashFunction);

            Assert.IsInstanceOf(expectedType, result);
        }
    }
}

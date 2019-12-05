using System;
using NIST.CVP.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KeyWrap.Tests
{
    [TestFixture,  FastCryptoTest]
    public class KeyWrapFactoryTests
    {

        private KeyWrapFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new KeyWrapFactory();
        }

        [Test]
        [TestCase(KeyWrapType.AES_KW, typeof(KeyWrapAes))]
        [TestCase(KeyWrapType.TDES_KW, typeof(KeyWrapTdes))]
        public void ShouldReturnCorrectImplementation(KeyWrapType keyWrapType, Type expectedType)
        {
            var result = _subject.GetKeyWrapInstance(keyWrapType);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldThrowExceptionWithInvalidKeyWrapType()
        {
            int invalidKeyWrap = -1;
            KeyWrapType keyWrapType = (KeyWrapType) invalidKeyWrap;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetKeyWrapInstance(keyWrapType));
        }
    }
}

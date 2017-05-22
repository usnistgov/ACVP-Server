using System;
using NIST.CVP.Crypto.KeyWrap.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.KeyWrap.Tests
{
    [TestFixture, UnitTest]
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

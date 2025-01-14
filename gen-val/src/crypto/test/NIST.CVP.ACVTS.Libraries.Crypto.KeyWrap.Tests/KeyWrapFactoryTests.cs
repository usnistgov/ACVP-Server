﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.KeyWrap.Enums;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KeyWrap.Tests
{
    [TestFixture, FastCryptoTest]
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

            Assert.That(result, Is.InstanceOf(expectedType));
        }

        [Test]
        public void ShouldThrowExceptionWithInvalidKeyWrapType()
        {
            int invalidKeyWrap = -1;
            KeyWrapType keyWrapType = (KeyWrapType)invalidKeyWrap;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetKeyWrapInstance(keyWrapType));
        }
    }
}

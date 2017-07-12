using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CMAC.Tests
{
    [TestFixture, UnitTest]
    public class CmacFactoryTests
    {
        private CmacFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new CmacFactory();
        }
        
        [Test]
        [TestCase(CmacTypes.AES128, typeof(CmacAes))]
        [TestCase(CmacTypes.AES192, typeof(CmacAes))]
        [TestCase(CmacTypes.AES256, typeof(CmacAes))]
        public void ShouldReturnProperCmacInstance(CmacTypes cmacType, Type expectedType)
        {
            var result = _subject.GetCmacInstance(cmacType);

            Assert.IsInstanceOf(expectedType, result);
        }

        [Test]
        public void ShouldReturnArgumentExceptionWhenInvalidEnum()
        {
            int i = -1;
            var badCmacType = (CmacTypes)i;

            Assert.Throws(typeof(ArgumentException), () => _subject.GetCmacInstance(badCmacType));
        }
    }
}

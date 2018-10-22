using System;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.Engines;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.CMAC.Tests
{
    [TestFixture, FastCryptoTest]
    public class CmacFactoryTests
    {
        private CmacFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
        }
        
        [Test]
        [TestCase(CmacTypes.AES128, typeof(CmacAes))]
        [TestCase(CmacTypes.AES192, typeof(CmacAes))]
        [TestCase(CmacTypes.AES256, typeof(CmacAes))]
        [TestCase(CmacTypes.TDES, typeof(CmacTdes))]
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

using System;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES.Tests
{
    [TestFixture,  FastCryptoTest]
    public class RijndaelFactoryTests
    {

        private RijndaelFactory _subject;
        private Mock<IRijndaelInternals> _mockRijndaelInternals;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRijndaelInternals = new Mock<IRijndaelInternals>();
            _subject = new RijndaelFactory(_mockRijndaelInternals.Object);
        }

        [Test]
        public void ShouldReturnRijndaelECB()
        {
            var result = _subject.GetRijndael(ModeValues.ECB);

            Assert.IsInstanceOf(typeof(RijndaelECB), result);
        }

        [Test]
        public void ShouldReturnRijndaelCBC()
        {
            var result = _subject.GetRijndael(ModeValues.CBC);

            Assert.IsInstanceOf(typeof(RijndaelCBC), result);
        }

        [Test]
        public void ShouldReturnRijndaelOFB()
        {
            var result = _subject.GetRijndael(ModeValues.OFB);

            Assert.IsInstanceOf(typeof(RijndaelOFB), result);
        }

        [Test]
        public void ShouldReturnRijndaelCFB1()
        {
            var result = _subject.GetRijndael(ModeValues.CFB1);

            Assert.IsInstanceOf(typeof(RijndaelCFB1), result);
        }

        [Test]
        public void ShouldReturnRijndaelCFB8()
        {
            var result = _subject.GetRijndael(ModeValues.CFB8);

            Assert.IsInstanceOf(typeof(RijndaelCFB8), result);
        }

        [Test]
        public void ShouldReturnRijndaelCFB128()
        {
            var result = _subject.GetRijndael(ModeValues.CFB128);

            Assert.IsInstanceOf(typeof(RijndaelCFB128), result);
        }

        [Test]
        public void ShouldReturnRijndaelCounter()
        {
            var result = _subject.GetRijndael(ModeValues.Counter);

            Assert.IsInstanceOf(typeof(RijndaelCounter), result);
        }

        [Test]
        public void ShouldReturnRijndaelCBCMac()
        {
            var result = _subject.GetRijndael(ModeValues.CBCMac);

            Assert.IsInstanceOf(typeof(RijndaelCBCMac), result);
        }

        [Test]
        public void ShouldReturnRijndaelCMAC()
        {
            var result = _subject.GetRijndael(ModeValues.CMAC);

            Assert.IsInstanceOf(typeof(RijndaelCMAC), result);
        }
        
        [Test]
        [TestCase(100)]
        [TestCase(-1)]
        public void ShouldReturnArgumentExceptionWithInvalidMode(int mode)
        {
            var badCast = (ModeValues)mode;

            Assert.Throws(
                typeof(ArgumentException), 
                () => _subject.GetRijndael(badCast)
            );
        }
    }
}

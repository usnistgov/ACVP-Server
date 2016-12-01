using Moq;
using NIST.CVP.Generation.AES;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES.Tests
{
    [TestFixture]
    public class RijndaelFactoryTests
    {

        private RijndaelFactory _sut;
        private Mock<IRijndaelInternals> _mockRijndaelInternals;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockRijndaelInternals = new Mock<IRijndaelInternals>();
            _sut = new RijndaelFactory(_mockRijndaelInternals.Object);
        }

        [Test]
        public void ShouldReturnRijndaelECB()
        {
            var result = _sut.GetRijndael(ModeValues.ECB);

            Assert.IsInstanceOf(typeof(RijndaelECB), result);
        }

        [Test]
        [TestCase(100)]
        [TestCase(-1)]
        public void ShouldReturnArgumentExceptionWithInvalidMode(int mode)
        {
            var badCast = (ModeValues)mode;

            Assert.Throws(
                typeof(ArgumentException), 
                () => _sut.GetRijndael(badCast)
            );
        }
    }
}

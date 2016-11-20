using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES.Tests
{
    [TestFixture]
    public class RijndaelTests
    {
        private Mock<IRijndaelInternals> _mockIRijnddaelInternals;
        private RijndaelTest _sut;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockIRijnddaelInternals = new Mock<IRijndaelInternals>();
            _sut = new RijndaelTest(_mockIRijnddaelInternals.Object);
        }

        [Test]
        public void ShouldInvokeKeyAddition()
        {
            _sut.KeyAddition(It.IsAny<byte[,]>(), It.IsAny<byte[,]>(), It.IsAny<int>());

            _mockIRijnddaelInternals.
                Verify(v => v.KeyAddition(It.IsAny<byte[,]>(), It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_sut.KeyAddition));
        }

        [Test]
        public void ShouldInvokeSubstitution()
        {
            _sut.Substitution(It.IsAny<byte[,]>(), It.IsAny<byte[]>(), It.IsAny<int>());

            _mockIRijnddaelInternals.
                Verify(v => v.Substitution(It.IsAny<byte[,]>(), It.IsAny<byte[]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_sut.Substitution));
        }

        [Test]
        public void ShouldInvokeShiftRow()
        {
            _sut.ShiftRow(It.IsAny<byte[,]>(), It.IsAny<int>(), It.IsAny<int>());

            _mockIRijnddaelInternals.
                Verify(v => v.ShiftRow(It.IsAny<byte[,]>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once,
                nameof(_sut.ShiftRow));
        }

        [Test]
        public void ShouldInvokeMixColumn()
        {
            _sut.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>());

            _mockIRijnddaelInternals.
                Verify(v => v.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_sut.MixColumn));
        }

        [Test]
        public void ShouldInvokeMultiply()
        {
            _sut.Multiply(It.IsAny<byte>(), It.IsAny<byte>());

            _mockIRijnddaelInternals.
                Verify(v => v.Multiply(It.IsAny<byte>(), It.IsAny<byte>()),
                Times.Once,
                nameof(_sut.Multiply));
        }
    }
}

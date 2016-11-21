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

        #region MakeKey
        static object[] makeKeyTestData = new object[]
        {
            new object[]
            {
                new byte[] { 1, 2, 3, 4, 5 },
                DirectionValues.Enrypt
            },
            new object[]
            {
                new byte[] { 1, 7, 13, 42, 55, 77, 100, 124, 125, 150 },
                DirectionValues.Decrypt
            }
        };
        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldSetBytes(byte[] keyData, DirectionValues direction)
        {
            var result = _sut.MakeKey(keyData, direction);

            Assert.AreEqual(keyData, result.Bytes);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldSetBlockLength(byte[] keyData, DirectionValues direction)
        {
            var result = _sut.MakeKey(keyData, direction);

            Assert.AreEqual(128, result.BlockLength);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldSetKeyDirection(byte[] keyData, DirectionValues direction)
        {
            var result = _sut.MakeKey(keyData, direction);

            Assert.AreEqual(direction, result.Direction);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldReturnAppropriateKeyLengthBasedOnBytesData(byte[] keyData, DirectionValues direction)
        {
            var result = _sut.MakeKey(keyData, direction);

            Assert.AreEqual(keyData.Length, result.Length);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldReturnComposedKeyWithMakeKey(byte[] keyData, DirectionValues direction)
        {
            var result = _sut.MakeKey(keyData, direction);

            Assert.IsNotNull(result.KeySchedule);
        }

        // @@@ TODO how to test generation of local "k" within MakeKey.  Pull out k generation into separate function to validate?
        #endregion MakeKey

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

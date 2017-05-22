using System;
using Moq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES.Tests
{
    [TestFixture, UnitTest]
    public class RijndaelTests
    {
        private Mock<IRijndaelInternals> _mockIRijndaelInternals;
        private RijndaelTest _subject;

        [OneTimeSetUp]
        public void Setup()
        {
            _mockIRijndaelInternals = new Mock<IRijndaelInternals>();
            _subject = new RijndaelTest(_mockIRijndaelInternals.Object);
        }

        [Test]
        public void ShouldThrowNotImplementedException()
        {
            Assert.Throws(
                typeof(NotImplementedException),
                () => _subject.BlockEncrypt(
                    new Cipher()
                    {
                        BlockLength = 8
                    }, 
                    new Key(),
                    new byte[] { }, 
                    8
                )
            );
        }

        #region MakeKey
        static object[] makeKeyTestData = new object[]
        {
            new object[]
            {
                new byte[] { 1, 2, 3, 4, 5 },
                DirectionValues.Encrypt
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
            var result = _subject.MakeKey(keyData, direction);

            Assert.AreEqual(keyData, result.Bytes);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldSetBlockLength(byte[] keyData, DirectionValues direction)
        {
            var result = _subject.MakeKey(keyData, direction);

            Assert.AreEqual(128, result.BlockLength);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldSetKeyDirection(byte[] keyData, DirectionValues direction)
        {
            var result = _subject.MakeKey(keyData, direction);

            Assert.AreEqual(direction, result.Direction);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldReturnAppropriateKeyLengthBasedOnBytesData(byte[] keyData, DirectionValues direction)
        {
            var result = _subject.MakeKey(keyData, direction);

            Assert.AreEqual(keyData.Length, result.Length);
        }

        [Test]
        [TestCaseSource(nameof(makeKeyTestData))]
        public void ShouldReturnComposedKeyWithMakeKey(byte[] keyData, DirectionValues direction)
        {
            var result = _subject.MakeKey(keyData, direction);

            Assert.IsNotNull(result.KeySchedule);
        }

        // @@@ TODO how to test generation of local "k" within MakeKey.  Pull out k generation into separate function to validate?
        #endregion MakeKey

        [Test]
        public void ShouldInvokeKeyAddition()
        {
            _subject.KeyAddition(It.IsAny<byte[,]>(), It.IsAny<byte[,]>(), It.IsAny<int>());

            _mockIRijndaelInternals.
                Verify(v => v.KeyAddition(It.IsAny<byte[,]>(), It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_subject.KeyAddition));
        }

        [Test]
        public void ShouldInvokeSubstitution()
        {
            _subject.Substitution(It.IsAny<byte[,]>(), It.IsAny<byte[]>(), It.IsAny<int>());

            _mockIRijndaelInternals.
                Verify(v => v.Substitution(It.IsAny<byte[,]>(), It.IsAny<byte[]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_subject.Substitution));
        }

        [Test]
        public void ShouldInvokeShiftRow()
        {
            _subject.ShiftRow(It.IsAny<byte[,]>(), It.IsAny<int>(), It.IsAny<int>());

            _mockIRijndaelInternals.
                Verify(v => v.ShiftRow(It.IsAny<byte[,]>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Once,
                nameof(_subject.ShiftRow));
        }

        [Test]
        public void ShouldInvokeMixColumn()
        {
            _subject.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>());

            _mockIRijndaelInternals.
                Verify(v => v.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Once,
                nameof(_subject.MixColumn));
        }

        [Test]
        public void ShouldInvokeMultiply()
        {
            _subject.Multiply(It.IsAny<byte>(), It.IsAny<byte>());

            _mockIRijndaelInternals.
                Verify(v => v.Multiply(It.IsAny<byte>(), It.IsAny<byte>()),
                Times.Once,
                nameof(_subject.Multiply));
        }
    }
}

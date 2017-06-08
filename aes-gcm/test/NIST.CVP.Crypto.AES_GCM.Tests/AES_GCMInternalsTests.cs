using System.Collections;
using System.Numerics;
using Moq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Math;
using NIST.CVP.Math.Helpers;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_GCM.Tests
{
    [TestFixture, UnitTest]
    public class AES_GCMInternalsTests
    {
        Mock<IRijndaelInternals> _mockIRijndaelInternals;
        Mock<IRijndaelFactory> _mockiRijndaelFactory;
        AES_GCMInternals _subject;
        Mock<AES_GCMInternals> _mockSubject;

        [SetUp]
        public void Setup()
        {
            _mockIRijndaelInternals = new Mock<IRijndaelInternals>();
            _mockiRijndaelFactory = new Mock<IRijndaelFactory>();
            _mockiRijndaelFactory
                .Setup(s => s.GetRijndael(It.IsAny<ModeValues>()))
                .Returns(new RijndaelECB(_mockIRijndaelInternals.Object));
            _subject = new AES_GCMInternals(new RijndaelFactory(new RijndaelInternals()));
            _mockSubject = new Mock<AES_GCMInternals>(_mockiRijndaelFactory.Object);
            _mockSubject.CallBase = true;
        }

        #region GetJ0
        /// <summary>
        /// With 96 length IV, j0 should be 32 0s (but first 0 is actually 1) + iv
        /// </summary>
        /// <param name="hString">The h string</param>
        /// <param name="ivString">The IV string</param>
        /// <param name="expectedString">The expected j0</param>
        [Test]
        [TestCase(
            "00000000",
            "110000000000000000000000000000000000000000000000000000000000000000000000000000000000000010111101",
            "10000000000000000000000000000000110000000000000000000000000000000000000000000000000000000000000000000000000000000000000010111101"
        )]
        [TestCase(
            "00000000",
            "101100111000111100001111100000111111000000111111100000001111111100000000111111111000000000111111",
            "10000000000000000000000000000000101100111000111100001111100000111111000000111111100000001111111100000000111111111000000000111111"
        )]
        public void ShouldReturnJ0With96BitIvWithNoExternalCalls(string hString, string ivString, string expectedString)
        {
            BitString h = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(hString));
            BitString iv = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(ivString));
            BitString expectedJ0 = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(expectedString));

            var result = _mockSubject.Object.Getj0(h, iv);

            Assert.AreEqual(expectedJ0, result, nameof(expectedJ0));
            _mockSubject.Verify(
                v => v.Ceiling(It.IsAny<int>(), It.IsAny<int>()),
                Times.Never,
                nameof(_mockSubject.Object.Ceiling)
            );
            _mockSubject.Verify(
                v => v.GHash(It.IsAny<BitString>(), It.IsAny<BitString>()),
                Times.Never,
                nameof(_mockSubject.Object.GHash)
            );
        }

        [Test]
        [TestCase("11000000", "11111000")]
        public void ShouldInvokeGHashWithAppropriateValuesWhenIvNot96Bits(string hString, string ivString)
        {
            BitString h = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(hString));
            BitString iv = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(ivString));
            BitString fakeGHashReturn = new BitString(5);

            _mockSubject
                .Setup(s => s.Ceiling(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(96);
            _mockSubject
                .Setup(s => s.GHash(It.IsAny<BitString>(), It.IsAny<BitString>()))
                .Returns(fakeGHashReturn);

            var result = _mockSubject.Object.Getj0(h, iv);

            var expectedS = 128 * 96 - iv.BitLength;
            var expectedX = iv.ConcatenateBits(new BitString(new BitArray(expectedS + 64))).ConcatenateBits(BitString.To64BitString(iv.BitLength));

            Assert.AreEqual(fakeGHashReturn, result, nameof(result));
            _mockSubject.Verify(
                v => v.Ceiling(iv.BitLength, 128),
                Times.Once,
                nameof(_mockSubject.Object.Ceiling)
            );
            _mockSubject.Verify(
                v => v.GHash(h, iv.ConcatenateBits(expectedX)),
                Times.Once,
                nameof(_mockSubject.Object.GHash)
            );
        }
        #endregion GetJ0

        #region Ceiling
        [Test]
        [TestCase(1, 5, 1)] // 1 / 5 = 0 (.2), 1 mod 5 = 1, 0+1 = 1
        [TestCase(200, 13, 16)] // 200 / 13 = 15 (15.38), 200 mod 13 = 5, 15+1 = 16
        public void ShouldReturnCeiling(int numerator, int denominator, int expectedCeiling)
        {
            var result = _subject.Ceiling(numerator, denominator);

            Assert.AreEqual(expectedCeiling, result);
        }
        #endregion Ceiling

        #region GHash
        [Test]
        [TestCase(128, false)]
        [TestCase(127, true)]
        [TestCase(129, true)]
        [TestCase(0, true)]
        public void GHashShouldReturnNullWhenHBitLengthNot128(int bitLength, bool shouldBeNull)
        {
            BitString h = new BitString(bitLength);
            BitString x = new BitString(128);

            var result = _subject.GHash(h, x);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }

        [Test]
        [TestCase(128, false)]
        [TestCase(256, false)]
        [TestCase(0, true)]
        [TestCase(127, true)]
        [TestCase(129, true)]
        public void GHashShouldReturnNullWhenXBitLengthNot0WhenModulod128(int bitLength, bool shouldBeNull)
        {
            BitString h = new BitString(128);
            BitString x = new BitString(bitLength);

            var result = _subject.GHash(h, x);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }

        [Test]
        [TestCase(128)]
        public void GHashShouldBlockProductSameNumberOfTimesAsXLengthDiv128(int xLength)
        {
            BitString h = new BitString(128);
            BitString x = new BitString(xLength);
            int expectedInvokes = xLength / 128;

            var result = _mockSubject.Object.GHash(h, x);

            _mockSubject.Verify(
                v => v.BlockProduct(It.IsAny<BitString>(), h),
                Times.Exactly(expectedInvokes),
                nameof(_mockSubject.Object.BlockProduct)
            );

        }
        #endregion GHash

        #region GCTR
        [Test]
        public void GCTRShouldReturnNullWhenIcbBitLengthNot128()
        {
            BitString icb = new BitString(127);
            BitString x = new BitString(128);
            Key key = new Key();

            var result = _subject.GCTR(icb, x, key);

            Assert.IsNull(result);
        }

        [Test]
        public void GCTRShouldReturnZeroLengthBitStringWhenXIsZeroLength()
        {

            BitString icb = new BitString(128);
            BitString x = new BitString(0);
            Key key = new Key();

            BitString expectation = new BitString(0);

            var result = _subject.GCTR(icb, x, key);

            Assert.AreEqual(expectation, result);
        }

        [Test]
        public void GCTRShouldCallInternalsExpectedNumberOfTimesCeilingLt1()
        {
            int icbLength = 128;
            int xLength = 128;
            int mockedCeiling = 0;

            BitString icb = new BitString(icbLength);
            BitString x = new BitString(xLength);
            Key key = new Key()
            {
                BlockLength = 128,
                Bytes = new byte[8],
                Direction = DirectionValues.Encrypt,
                KeySchedule = new RijndaelKeySchedule(128, 128, new byte[4,8])
            };

            _mockSubject
                .Setup(s => s.Ceiling(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockedCeiling);

            var result = _mockSubject.Object.GCTR(icb, x, key);

            _mockSubject.Verify(
                v => v.Ceiling(x.BitLength, 128),
                Times.Once,
                nameof(_mockSubject.Object.Ceiling)
            );
            _mockSubject.Verify(
                v => v.inc_s(It.IsAny<int>(), It.IsAny<BitString>()), 
                Times.Exactly(mockedCeiling), 
                nameof(_mockSubject.Object.inc_s)
            );
            _mockIRijndaelInternals.Verify(
                v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()), 
                Times.Exactly(mockedCeiling + 1), 
                nameof(_mockIRijndaelInternals.Object.EncryptSingleBlock));
        }

        [Test]
        [TestCase(128, 16384, 128)]
        public void GCTRShouldCallInternalsExpectedNumberOfTimesCeilingGt1(int icbLength, int xLength, int mockedCeiling)
        {
            BitString icb = new BitString(icbLength);
            BitString x = new BitString(xLength);
            Key key = new Key()
            {
                BlockLength = 128,
                Bytes = new byte[8],
                Direction = DirectionValues.Encrypt,
                KeySchedule = new RijndaelKeySchedule(128, 128, new byte[4, 8])
            };

            _mockSubject
                .Setup(s => s.Ceiling(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(mockedCeiling);

            var result = _mockSubject.Object.GCTR(icb, x, key);

            _mockSubject.Verify(
                v => v.Ceiling(x.BitLength, 128),
                Times.Once,
                nameof(_mockSubject.Object.Ceiling)
            );
            _mockSubject.Verify(
                v => v.inc_s(It.IsAny<int>(), It.IsAny<BitString>()),
                Times.Exactly(mockedCeiling - 1),
                nameof(_mockSubject.Object.inc_s)
            );
            _mockIRijndaelInternals.Verify(
                v => v.EncryptSingleBlock(It.IsAny<byte[,]>(), It.IsAny<Key>()),
                Times.Exactly(mockedCeiling),
                nameof(_mockIRijndaelInternals.Object.EncryptSingleBlock));
        }
        #endregion GCTR

        #region inc_s
        [Test]
        public void inc_sShouldReturnNullWhenBitStringLengthLtS()
        {
            BitString bs = new BitString(128);
            int s = 129;

            var result = _subject.inc_s(s, bs);

            Assert.IsNull(result);
        }

        /// <summary>
        ///     1. Get a big integer that is comprised of ((the "s" least significant bits of "X") + 1)
        ///     2. "lsp" = "lsp" mod (1 left shifted "s")
        ///     3. create new bitstring "bitsToAppend" that is the first (least significant bits) "s" digits of lsp
        ///     4. return (the "X" length - "s" most significant bits of "X" concatenated with "lsp"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="onesAndZeroes"></param>
        [Test]
        //// 1.
        //// lsp = LSB("s", "X").ToBigInt + BigInt(1)
        //// lsp = LSB(1 + "1111").ToBigInt + BigInt(1)
        //// lsp = "1".ToBigInt + BigInt(1)
        //// lsp = 1 + 1
        //// lsp = 2

        //// 2.
        //// lsp = lsp % (1 << "s")
        //// lsp = 2 % (1 << 1)
        //// lsp = 2 % 2
        //// lsp = 0

        //// 3.
        //// "bitsToAppend" = BitString("lsp", "s")
        //// "bitsToAppend" = BitString(0, 1)
        //// "bitsToAppend" = 0 (00000000)

        //// 4.
        //// return (MSB("X.length" - "s", "X") + "bitsToAppend")
        //// return (MSB(4 - 1, 1111) + 00000000)
        //// return (MSB(3, 1111) + 00000000)
        //// return 111 + 00000000
        //// return 00000000111
        //[TestCase(1, "1111", "00000000111")]
        // 1.
        // lsp = LSB("s", "X").ToBigInt + BigInt(1)
        // lsp = LSB(5 + "10101010").ToBigInt + BigInt(1)
        // lsp = "10101".ToBigInt + BigInt(1)
        // lsp = 21 + 1
        // lsp = 22

        // 2.
        // lsp = lsp % (1 << "s")
        // lsp = 22 % (1 << 5)
        // lsp = 22 % 16
        // lsp = 22

        // 3.
        // "bitsToAppend" = BitString("lsp", "s")
        // "bitsToAppend" = BitString(22, 5)
        // "bitsToAppend" = "01101000" (LSb)

        // 4.
        // return (MSB("X.length" - "s", "X") + "bitsToAppend")
        // return (MSB(8 - 5, 10101010) + 01101)
        // return (MSB(3, 10101010) + 01101000)
        // return 010 + 01101000
        // return 01101000010
        [TestCase(5, "10101010", "01101000010")]
        public void ShouldCorrectlyInc_s(int s, string onesAndZeroes, string expectationOnesAndZeroes)
        {
            BitString X = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(onesAndZeroes));
            BitString expectation = new BitString(MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(expectationOnesAndZeroes));

            var result = _subject.inc_s(s, X);

            Assert.AreEqual(expectation, result);
        }
        #endregion inc_s

        #region LSB
        [Test]
        [TestCase(new bool[] { false, false, false, false, true, true, true, true }, 
            4,
            new bool[] { false, false, false, false }
        )]
        [TestCase(new bool[] { false, true, false, false, true, true, true, true },
            3,
            new bool[] { false, true, false }
        )]
        public void ShouldReturnLeastSignificantBits(bool[] bitsInLSb, int numberOfBits, bool[] expectedBits)
        {
            BitString bs = new BitString(new BitArray(bitsInLSb));
            BitString expectation = new BitString(new BitArray(expectedBits));
            var result = _subject.LSB(numberOfBits, bs);

            Assert.AreEqual(expectation, result);
        }
        #endregion LSB

        #region MSB
        [Test]
        [TestCase(new bool[] { false, false, false, false, true, true, true, true },
            4,
            new bool[] { true, true, true, true }
        )]
        [TestCase(new bool[] { false, true, false, false, true, false, true, true },
            3,
            new bool[] { false, true, true }
        )]
        public void ShouldReturnMostSignificantBits(bool[] bitsInLSb, int numberOfBits, bool[] expectedBits)
        {
            BitString bs = new BitString(new BitArray(bitsInLSb));
            BitString expectation = new BitString(new BitArray(expectedBits));
            var result = _subject.MSB(numberOfBits, bs);

            Assert.AreEqual(expectation, result);
        }
        #endregion MSB

        #region BlockProduct
        [Test]
        [TestCase(0, 0, true)]
        [TestCase(128, 127, true)]
        [TestCase(127, 128, true)]
        [TestCase(128, 128, false)]
        public void BlockProductShouldReturnNullIfXOrYBitLengthNot128(int xLength, int yLength, bool shouldBeNull)
        {
            BitString x = new BitString(xLength);
            BitString y = new BitString(yLength);

            var result = _subject.BlockProduct(x, y);

            Assert.IsTrue(shouldBeNull ? result == null : result != null);
        }

        [Test]
        public void ShouldReturnZeroBitStringWhenXZeroBitStringsProvided()
        {
            BigInteger expectation = new BigInteger(0);

            BitString x = new BitString(128);
            BitString y = x.XOR(x);
            var result = _subject.BlockProduct(x, y);

            Assert.AreEqual(expectation, result.ToBigInteger());
        }
        #endregion BlockProduct
    }
}

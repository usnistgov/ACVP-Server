using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Numerics;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class BitStringTests
    {

        #region ctor
        [Test]
        public void ShouldCreateInstanceWithLength()
        {
            // Arrange
            int length = 10;

            // Act
            BitString sut = new BitString(length);

            // Assert
            Assert.AreEqual(length, sut.Length);
        }

        [Test]
        public void ShouldCreateInstanceWithByteArray()
        {
            // Arrange
            byte[] bytes = new byte[] { 1, 2, 3, 4 };

            // Act
            BitString sut = new BitString(bytes);
            var results = sut.ToBytes();

            // Assert
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(bytes[i], results[i]);
            }
        }

        [Test]
        public void ShouldCreateInstanceWithBitArray()
        {
            // Arrange
            bool[] bits = new bool[] { true, false, true, true };

            // Act
            BitString sut = new BitString(new BitArray(bits));

            // Assert
            for (int i = 0; i < sut.Length; i++)
            {
                Assert.AreEqual(bits[i], sut.Bits[i]);
            }
        }

        [Test]
        public void ShouldCreateInstanceWithBigInteger()
        {
            // Arrange
            byte[] bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            int bitsToAByte = 8;
            int totalBits = bytes.Length * bitsToAByte;
            BigInteger bi = new BigInteger(bytes);

            // Act
            BitString sut = new BitString(bi, totalBits);
            var resultBytes = sut.ToBytes();

            // Assert
            for (int i = 0; i < resultBytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], resultBytes[i]);
            }
        }

        [Test]
        public void ShouldCreateInstanceWithBigIntegerAndSetLength()
        {
            // Arrange
            byte[] bytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            BigInteger bi = new BigInteger(bytes);
            int bitsToAByte = 8;
            int totalBits = bytes.Length * bitsToAByte;
            int setBitLengthTo = totalBits + bitsToAByte; // one additional byte over what's provided in byte array

            // Act
            BitString sut = new BitString(bi, setBitLengthTo);

            // Assert
            Assert.IsTrue(setBitLengthTo == sut.Bits.Length, $"Resulting bits length should be {setBitLengthTo}");
        }

        [Test]
        public void ShouldAddFalseBitsAfterBigInt()
        {
            // Arrange
            byte[] bytes = new byte[] { 1 };
            BigInteger bi = new BigInteger(bytes);
            int bitsToAByte = 8;
            int totalBits = bytes.Length * bitsToAByte;
            int setBitLengthTo = totalBits + bitsToAByte; // one additional byte over what's provided in byte array

            // Act
            BitString sut = new BitString(bi, setBitLengthTo);
            var results = sut.ToBytes();

            // Assert
            Assume.That(results.Length == 2);
            Assert.IsTrue(results[0] == 1);
            Assert.IsTrue(results[1] == 0);
        }

        [Test]
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "DC 05")]
        [TestCase(int.MaxValue, "FF FF FF 7F")]
        public void ShouldCreateInstanceFromHexStringLSB(int testExpectation, string hexValue)
        {
            // Arrange
            var expectedBitString = BitString.To64BitString(testExpectation);

            // Act
            var sut = new BitString(hexValue);
            var result = sut.ToBigInteger();
            var bigIntByteArray = result.ToByteArray();
            var createdBitStringFromBigIntByteArray = new BitString(bigIntByteArray);

            // Assert
            for (int i = 0; i < createdBitStringFromBigIntByteArray.Bits.Length; i++)
            {
                Assert.AreEqual(expectedBitString.Bits[i], createdBitStringFromBigIntByteArray.Bits[i]);
            }
        }

        [Test]
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "DC 05")]
        [TestCase(int.MaxValue, "FF FF FF 7F")]
        public void ShouldCreateInstanceFromHexString(long testExpectation, string hexValue)
        {
            // Act
            BitString sut = new BitString(hexValue);
            var result = sut.ToBigInteger();
            var comparison = result.CompareTo(testExpectation);

            // Assert
            Assert.IsTrue(comparison == 0);
        }
        #endregion ctor

        #region Equals
        [Test]
        [TestCase(new bool[] { true }, new bool[] { true })]
        [TestCase(new bool[] { false }, new bool[] { false })]
        [TestCase(new bool[] { false, true, true, true }, new bool[] { false, true, true, true })]
        public void EqualsMethodReturnsTrueForLikeBoolArrays(bool[] workingArray, bool[] compareArray)
        {
            // Arrange
            BitArray workingBitArray = new BitArray(workingArray);
            BitArray compareBitArray = new BitArray(compareArray);

            BitString workingBs = new BitString(workingBitArray);
            BitString compareBs = new BitString(compareBitArray);

            // Act
            var results = workingBs.Equals(compareBs);

            // Assert
            Assert.IsTrue(results);
        }

        [Test]
        public void EqualsMethodReturnsFalseWhenCompareObjectIsntAppropriateType()
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { true }));
            int foo = 5;

            // Act
            var results = bs.Equals(foo);

            // Assert
            Assert.IsFalse(results);
        }

        [Test]
        [TestCase(new bool[] { true }, new bool[] { true, true })]
        public void EqualsMethodReturnsFalseWhenArraysAreOfDifferentLength(bool[] workingArray, bool[] compareArray)
        {
            // Arrange
            BitArray workingBitArray = new BitArray(workingArray);
            BitArray compareBitArray = new BitArray(compareArray);

            BitString workingBs = new BitString(workingBitArray);
            BitString compareBs = new BitString(compareBitArray);

            // Act
            var results = workingBs.Equals(compareBs);

            // Assert
            Assert.IsFalse(results);
        }

        [Test]
        [TestCase(new bool[] { true }, new bool[] { false })]
        [TestCase(new bool[] { true, true, true }, new bool[] { true, false, true })]
        public void EqualsMethodReturnsFalseWhenArraysAreOfSimilarLengthDifferingValues(bool[] workingArray, bool[] compareArray)
        {
            // Arrange
            BitArray workingBitArray = new BitArray(workingArray);
            BitArray compareBitArray = new BitArray(compareArray);

            BitString workingBs = new BitString(workingBitArray);
            BitString compareBs = new BitString(compareBitArray);

            // Act
            var results = workingBs.Equals(compareBs);

            // Assert
            Assert.IsFalse(results);
        }
        #endregion Equals

        #region ToBytes
        [Test]
        // less than one byte
        [TestCase(new bool[] { true }, 1)]
        [TestCase(new bool[] { false, false, true }, 4)]
        [TestCase(new bool[] { false, false, false, true }, 8)]
        [TestCase(new bool[] { true, false, false, false, true }, 17)]
        // one byte
        [TestCase(new bool[] { false, false, false, false, false, false, false, true }, 128)]
        // one byte plus some bits
        [TestCase(new bool[] { true, false, false, false, false, false, true, false, false, false, false, false, false, false, true }, 16449)]
        // two bytes
        [TestCase(new bool[] { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true }, 32896)]
        // three bytes
        [TestCase(new bool[] { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true }, 8421504)]
        public void ToBytesConvertsBinaryToBytes(bool[] bits, int expectedResult)
        {
            // Arrange
            BitArray bitArray = new BitArray(bits);
            BitString bitString = new BitString(bitArray);

            // Act
            var results = bitString.ToBytes();

            // Assert
            var bytes = BitConverter.GetBytes(expectedResult);
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(bytes[i], results[i]);
            }
        }

        [Test]
        [TestCase(long.MinValue)]
        [TestCase(long.MinValue + 42)]
        [TestCase(long.MaxValue - 42)]
        [TestCase(long.MaxValue)]
        public void ToBytesLargerThanFourBytes(long largeNumber)
        {
            var bytes = BitConverter.GetBytes(largeNumber);

            BitArray bitArray = new BitArray(bytes);
            BitString bitString = new BitString(bitArray);

            var results = bitString.ToBytes();

            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(bytes[i], results[i]);
            }
        }

        [Test]
        // One Byte w/o 8 bits
        [TestCase(new bool[] { true })]
        [TestCase(new bool[] { false, false, true })]
        [TestCase(new bool[] { false, false, false, true })]
        [TestCase(new bool[] { true, false, false, false, true })]
        // One byte w/ 8 bits
        [TestCase(new bool[] { false, false, false, false, false, false, false, true })]
        // Two bytes w/o 16 bits
        [TestCase(new bool[] { true, false, false, false, false, false, true, false, false, false, false, false, false, false, true })]
        // Two bytes w/ 16 bits
        [TestCase(new bool[] { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true })]
        // Three bytes
        [TestCase(new bool[] { false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true })]
        public void ToBytesReturnsBytesInReverseOrderWhenSpecified(bool[] bits)
        {
            // Arrange
            BitArray bitArray = new BitArray(bits);
            BitString bitString = new BitString(bitArray);

            // Act
            var results = bitString.ToBytes();
            var resultsReverse = bitString.ToBytes(true);

            // Assert
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(results[i], resultsReverse[results.Length - 1 - i]);
            }
        }

        [Test]
        [TestCase(1, new byte[] { 1 })]
        [TestCase(255, new byte[] { 255 })]
        [TestCase(256, new byte[] { 0, 1 })]
        [TestCase(1500, new byte[] { 220, 5 })]
        public void ToBytesShouldBeInLeastSignificantByteOrder(int valueToToBytes, byte[] expectedByteArrayOrder)
        {
            // Arrange
            BitString bs = new BitString(BitConverter.GetBytes(valueToToBytes));

            // Act
            var sut = bs.ToBytes();

            // Assert
            for (int i = 0; i < expectedByteArrayOrder.Length; i++)
            {
                Assert.AreEqual(expectedByteArrayOrder[i], sut[i]);
            }
        }
        #endregion ToBytes

        #region ToString
        [Test]
        [TestCase(new bool[] { true })] // 1 bit
        [TestCase(new bool[] { true, true, true, true })] // 4 bits
        [TestCase(new bool[] { true, true, true, true, true, true, true, true })] // 8 bits
        public void ToStringShouldNotContainAnySpacesWhenEightBitsOrFewer(bool[] bits)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(bits));

            // Act
            var result = bs.ToString();

            // Assert
            Assert.IsFalse(result.Contains(" "));
        }

        [Test]
        [TestCase(new bool[] { true, true, true, true }, 0)] // 4 bits
        [TestCase(new bool[] { true, true, true, true, true, true, true, true }, 0)] // 8 bits
        [TestCase(new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }, 1)] // 16 bits
        [TestCase(new bool[] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true }, 2)] // 17 bits
        public void ToStringShouldContainOneSpaceForEachByteMinusOne(bool[] bits, int numberOfSpaces)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(bits));

            // Act
            var result = bs.ToString();

            // Assert
            Assert.AreEqual(numberOfSpaces, result.Count(c => c == ' '));
        }

        [Test]
        // Single Byte
        [TestCase(1, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000001")]
        [TestCase(254, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111110")]
        [TestCase(255, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 11111111")]
        // Multiple bytes
        [TestCase(256, "00000000 00000000 00000000 00000000 00000000 00000000 00000001 00000000")]
        [TestCase(123456, "00000000 00000000 00000000 00000000 00000000 00000001 11100010 01000000")]
        [TestCase(123457, "00000000 00000000 00000000 00000000 00000000 00000001 11100010 01000001")]
        [TestCase(long.MaxValue, "01111111 11111111 11111111 11111111 11111111 11111111 11111111 11111111")]
        public void ToStringShouldReturnABitRepresentationWithASpaceEveryEightBits(long toStringNumber, string expectedToString)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(toStringNumber);

            BitArray bitArray = new BitArray(bytes);
            BitString bitString = new BitString(bitArray);

            // Act
            var results = bitString.ToString();

            // Assert
            Assert.AreEqual(expectedToString, results);
        }
        #endregion ToString

        #region Set
        [Test]
        public void SetShouldReturnFalseWhenIndexGtLength()
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { false, false, false }));
            int length = bs.Length;

            // Act
            var result = bs.Set(length, true);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SetShouldReturnFalseWhenIndexLtZero()
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { false, false, false }));
            int length = bs.Length;

            // Act
            var result = bs.Set(-1, true);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void SetShouldReturnTrueWhenIndexInRange()
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { false, false, false }));
            int length = bs.Length;

            // Act
            var result = bs.Set(length - 1, true);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(new bool[] { false, false, false }, new bool[] { false, false, true }, 2, true)]
        [TestCase(new bool[] { true, false, false }, new bool[] { false, false, false }, 0, false)]
        public void SetShouldChangeBitAtIndex(bool[] original, bool[] expected, int indexToSet, bool valueToUseInSet)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(original));

            // Act
            var result = bs.Set(indexToSet, valueToUseInSet);

            // Assert
            Assert.AreEqual(new BitString(new BitArray(expected)), bs);
        }
        #endregion Set

        #region To64BitString
        [Test]
        // Note "ToString" representation as MSb, internal storage as LSb
        [TestCase(1, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000001")]
        [TestCase(4, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 00000100")]
        [TestCase(128, "00000000 00000000 00000000 00000000 00000000 00000000 00000000 10000000")]
        [TestCase(1024, "00000000 00000000 00000000 00000000 00000000 00000000 00000100 00000000")]
        [TestCase(1500, "00000000 00000000 00000000 00000000 00000000 00000000 00000101 11011100")]
        [TestCase(int.MaxValue, "00000000 00000000 00000000 00000000 01111111 11111111 11111111 11111111")]
        public void To64BitStringReturnsIntAsBitStringWith64Bits(int numberToConvert, string toStringRepresentation)
        {
            var result = BitString.To64BitString(numberToConvert);
            Assert.AreEqual(toStringRepresentation, result.ToString());
        }
        #endregion To64BitString

        #region ConcatenateBits
        [Test]
        [TestCase(
            new bool[] { false, false, false, true },
            new bool[] { false, true, true, false },
            new bool[] { false, true, true, false, false, false, false, true }
        )]
        [TestCase(
            new bool[] { false, false, false, false, false, true, false, false },
            new bool[] { true, true, true, true, false, true, true },
            new bool[] { true, true, true, true, false, true, true, false, false, false, false, false, true, false, false }
        )]
        public void ConcatenateBitsAppendsRightSideBitsToLeft(bool[] leftSide, bool[] rightSide, bool[] expectedResult)
        {
            // Arrange
            BitString lsBs = new BitString(new BitArray(leftSide));
            BitString rsBs = new BitString(new BitArray(rightSide));

            // Act
            BitString results = BitString.ConcatenateBits(lsBs, rsBs);

            // Assert
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(expectedResult[i], results.Bits[i]);
            }
        }
        #endregion ConcatenateBits

        #region LeftMost
        [Test]
        [TestCase(
        new bool[] { false, true, false, true, false, true, false, true },
        1,
        new bool[] { true }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            4,
            new bool[] { false, true, false, true }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            5,
            new bool[] { true, false, true, false, true }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            8,
            new bool[] { false, true, false, true, false, true, false, true }
        )]
        public void LeftmostShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = bs.Leftmost(numberOfBits);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }
        #endregion LeftMost

        #region RightMost
        [Test]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            1,
            new bool[] { false }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            4,
            new bool[] { false, true, false, true }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            5,
            new bool[] { false, true, false, true, false }
        )]
        [TestCase(
            new bool[] { false, true, false, true, false, true, false, true },
            8,
            new bool[] { false, true, false, true, false, true, false, true }
        )]
        public void RightmostShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = bs.Rightmost(numberOfBits);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }
        #endregion RightMost

        #region SubString
        [Test]
        [TestCase(new bool[] { true, false, true }, new bool[] { false }, 1, 1)]
        [TestCase(new bool[] { true, false, true }, new bool[] { true, false }, 0, 2)]
        [TestCase(new bool[] { true, false, true }, new bool[] { true, false, true }, 0, 3)]
        public void SubStringReturnsCorrectBitsWhenInvokedWithValidParamters(bool[] testBitString, bool[] expectedBitString, int startIndex, int numberOfBits)
        {
            // Arrange
            BitString testBs = new BitString(new BitArray(testBitString));
            BitString expectedBs = new BitString(new BitArray(expectedBitString));

            // Act
            var results = testBs.Substring(startIndex, numberOfBits);

            // Assert
            Assert.AreEqual(expectedBs, results);
        }

        [Test]
        public void StaticSubStringThrowsArgumentOutOfRangeExceptionWhenStartIndexLessThanZero()
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { true }));

            // Act / Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => BitString.Substring(bs, -1, 0));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public void StaticSubStringThrowsArgumentOutOfRangeExceptionWhenStartIndexGreaterThanBitStringLength(int startIndex)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { true }));

            // Act / Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => BitString.Substring(bs, startIndex, 1));
        }

        [Test]
        [TestCase(2, 2)]
        [TestCase(1, 3)]
        [TestCase(0, 4)]
        public void StaticSubStringThrowsArgumentOutOfRangeExceptionWhenStartIndexPlusNumberOfBitsIsGreaterThanLength(int startIndex, int numberOfBits)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { true, true, true }));

            // Act / Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => BitString.Substring(bs, startIndex, numberOfBits));
        }

        [Test]
        [TestCase(
            new bool[] { true, false, true },
            new bool[] { true, false },
            0,
            2
        )]
        [TestCase(
            new bool[] { false, false, false, true },
            new bool[] { true },
            3,
            1
        )]
        public void StaticSubStringReturnsCorrectBitsWhenInvokedWithValidParamters(bool[] testBitString, bool[] expectedBitString, int startIndex, int numberOfBits)
        {
            // Arrange
            BitString testBs = new BitString(new BitArray(testBitString));
            BitString expectedBs = new BitString(new BitArray(expectedBitString));

            // Act
            var results = BitString.Substring(testBs, startIndex, numberOfBits);

            // Assert
            Assert.AreEqual(expectedBs, results);
        }
        #endregion SubString

        #region XOR
        [Test]
        [TestCase(
            new bool[] { true, true, false },
            new bool[] { true, false, false },
            new bool[] { false, true, false }
        )]
        public void XORShouldReturnNewBitStringRepresentingXOROfTwoBitStrings(bool[] inputA, bool[] inputB, bool[] expectedResult)
        {
            // Arrange
            BitString bsA = new BitString(new BitArray(inputA));
            BitString bsB = new BitString(new BitArray(inputB));
            BitString expectedXorBs = new BitString(new BitArray(expectedResult));

            // Act
            var results = BitString.XOR(bsA, bsB);

            // Assert
            Assert.AreEqual(expectedXorBs, results);
        }

        [Test]
        //[TestCase(
        //            new bool[] { true, true },
        //            new bool[] { true },
        //            new bool[] { true, false }
        //        )]
        //[TestCase(
        //            new bool[] { true, false },
        //            new bool[] { true },
        //            new bool[] { true, true }
        //        )]
        //[TestCase(
        //            new bool[] { false, true },
        //            new bool[] { true },
        //            new bool[] { false, false }
        //        )]
        [TestCase(
                    new bool[] { true, true },
                    new bool[] { true },
                    new bool[] { false, true }
                )]
        [TestCase(
                    new bool[] { true, false },
                    new bool[] { true },
                    new bool[] { false, false }
                )]
        [TestCase(
                    new bool[] { false, true },
                    new bool[] { true },
                    new bool[] { true, true }
                )]
        public void XORInstanceShouldPadZeroesForShorterBitStringAndReturnNewBitString(bool[] inputA, bool[] inputB, bool[] expectedResult)
        {
            // Arrange
            BitString bsA = new BitString(new BitArray(inputA));
            BitString bsB = new BitString(new BitArray(inputB));
            BitString expectedXorBs = new BitString(new BitArray(expectedResult));

            // Act
            var results = bsA.XOR(bsB);

            // Assert
            Assert.AreEqual(expectedXorBs, results);
        }

        [Test]
        //[TestCase(
        //    new bool[] { true, true },
        //    new bool[] { true },
        //    new bool[] { true, false }
        //)]
        //[TestCase(
        //    new bool[] { true, false },
        //    new bool[] { true },
        //    new bool[] { true, true }
        //)]
        //[TestCase(
        //    new bool[] { false, true },
        //    new bool[] { true },
        //    new bool[] { false, false }
        //)]
        //[TestCase(
        //    new bool[] { true },
        //    new bool[] { true, false },
        //    new bool[] { true, true }
        //)]
        //[TestCase(
        //    new bool[] { true, false, true, false, true },
        //    new bool[] { false, true, true },
        //    new bool[] { true, false, true, true, false }
        //)]
        [TestCase(
            new bool[] { true, true },
            new bool[] { true },
            new bool[] { false, true }
        )]
        [TestCase(
            new bool[] { true, false },
            new bool[] { true },
            new bool[] { false, false }
        )]
        [TestCase(
            new bool[] { false, true },
            new bool[] { true },
            new bool[] { true, true }
        )]
        [TestCase(
            new bool[] { true },
            new bool[] { true, false },
            new bool[] { false, false }
        )]
        [TestCase(
            new bool[] { true, false, true, false, true },
            new bool[] { false, true, true },
            new bool[] { true, true, false, false, true }
        )]
        public void XORShouldPadZeroesForShorterBitStringAndReturnNewBitString(bool[] inputA, bool[] inputB, bool[] expectedResult)
        {
            // Arrange
            BitString bsA = new BitString(new BitArray(inputA));
            BitString bsB = new BitString(new BitArray(inputB));
            BitString expectedXorBs = new BitString(new BitArray(expectedResult));

            // Act
            var results = BitString.XOR(bsA, bsB);

            // Assert
            Assert.AreEqual(expectedXorBs, results);
        }
        #endregion XOR

        #region ToBigInteger
        [Test]
        [TestCase(1)]
        [TestCase(250)]
        [TestCase(1024)]
        [TestCase(int.MaxValue)]
        public void ToBigIntegerReturnsBigIntegerBasedOnBytes(int testInt)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(testInt);
            BitString bs = new BitString(bytes);
            BigInteger expectedBigInt = new BigInteger(testInt);

            // Act
            var result = bs.ToBigInteger();

            // Assert
            Assert.AreEqual(expectedBigInt, result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(250)]
        [TestCase(1024)]
        [TestCase(int.MaxValue)]
        [TestCase(long.MaxValue)]
        public void ToBigIntegerReturnsBigIntegerBasedOnBytes(long testLong)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(testLong);
            BitString bs = new BitString(bytes);
            BigInteger expectedBigInt = new BigInteger(testLong);

            // Act
            var result = bs.ToBigInteger();

            // Assert
            Assert.AreEqual(expectedBigInt, result);
        }
        #endregion ToBigInteger

        #region ToHex
        [Test]
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "DC05")]
        [TestCase(int.MaxValue, "FFFFFF7F")]
        public void ToHexShouldReturnHexStringOfBitString(int testLong, string expectedHex)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(testLong);
            var bs = new BitString(bytes);

            // Act
            var results = bs.ToHex();

            // Assert
            // Note LSB
            Assert.IsTrue(results.StartsWith(expectedHex.ToUpper()));
        }
        #endregion ToHex

        // TODO - does this method do anything outside of the scope of itself currently?
        #region ToDigit
        #endregion ToDigit
    }
}

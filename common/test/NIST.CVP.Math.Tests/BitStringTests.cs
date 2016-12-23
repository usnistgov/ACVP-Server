using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using System.Numerics;
using NIST.CVP.Math.Helpers;

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
            BitString subject = new BitString(length);

            // Assert
            Assert.AreEqual(length, subject.BitLength);
        }

        [Test]
        [TestCase(new byte[] { 1 })]
        [TestCase(new byte[] { 1, 2, 3, 4 })]
        [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 })]
        [TestCase(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
        public void ShouldCreateInstanceWithByteArray(byte[] bytes)
        {
            // Act
            BitString subject = new BitString(bytes);
            var results = subject.ToBytes();

            // Assert
            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(bytes[i], results[i]);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(254)]
        [TestCase(255)]
        [TestCase(256)]
        [TestCase(1500)]
        [TestCase(int.MaxValue)]
        public void ShouldCreateInstanceWithByteArrayAndExpectedValue(int testInt)
        {
            var bytes = BitConverter.GetBytes(testInt).Reverse().ToArray();

            var subject = new BitString(bytes);
            Assert.AreEqual(new BigInteger(testInt), subject.ToBigInteger());
        }

        [Test]
        public void ShouldCreateInstanceWithBitArray()
        {
            // TODO
            // Arrange
            bool[] bits = new bool[] { true, false, true, true };

            // Act
            BitString subject = new BitString(new BitArray(bits));

            // Assert
            for (int i = 0; i < subject.BitLength; i++)
            {
                Assert.AreEqual(bits[i], subject.Bits[i]);
            }
        }

        [Test]
        [TestCase(1)]
        [TestCase(254)]
        [TestCase(255)]
        [TestCase(256)]
        [TestCase(1500)]
        [TestCase(int.MaxValue)]
        public void ShouldCreateInstanceWithBitArrayAndExpectedValue(int testInt)
        {
            // Arrange
            var bytesInMSB = BitConverter.GetBytes(testInt).Reverse().ToArray();
            BigInteger expectedBi = new BigInteger(testInt);

            // Act
            BitString subject = new BitString(bytesInMSB);
            var result = subject.ToBigInteger();

            // Assert
            Assert.AreEqual(expectedBi, result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(255)]
        [TestCase(256)]
        [TestCase(1500)]
        [TestCase(int.MaxValue)]
        public void ShouldCreateInstanceWithBigInteger(int testInt)
        {
            // Arrange
            var testBigInt = new BigInteger(testInt);

            // Act
            BitString subject = new BitString(testBigInt);
            var resultBigInt = subject.ToBigInteger();

            // Assert
            Assert.AreEqual(testBigInt, resultBigInt);
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
            BitString subject = new BitString(bi, setBitLengthTo);

            // Assert
            Assert.AreEqual(setBitLengthTo, subject.Bits.Length, $"Resulting bits length should be {setBitLengthTo}");

        }

        [Test]
        public void ShouldAddFalseBitsBeforeBigInt()
        {
            // Arrange
            byte[] bytes = new byte[] { 1 };
            BigInteger bi = new BigInteger(bytes);
            int bitsToAByte = 8;
            int totalBits = bytes.Length * bitsToAByte;
            int setBitLengthTo = totalBits + bitsToAByte; // one additional byte over what's provided in byte array

            // Act
            BitString subject = new BitString(bi, setBitLengthTo);
            var results = subject.ToBytes();

            // Assert
            Assume.That(results.Length == 2);
            Assert.IsTrue(results[0] == 0);
            Assert.IsTrue(results[1] == 1);
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        public void ShouldCreateEmptyBitStringIfEmptyOrNullHexSupplied(string hexValue)
        {
            var subject = new BitString(hexValue);
            Assert.IsNotNull(subject);
            Assert.AreEqual(0, subject.BitLength);
        }

        [Test]
        // MSB
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "05 DC")]
        [TestCase(int.MaxValue, "7F FF FF FF")]
        //[Test]
        //// LSB
        //[TestCase(1, "01")]
        //[TestCase(10, "0A")]
        //[TestCase(15, "0F")]
        //[TestCase(1500, "DC 05")]
        //[TestCase(int.MaxValue, "FF FF FF 7F")]
        public void ShouldCreateInstanceFromHexStringInt(int testExpectation, string hexValue)
        {
            // Arrange
            var expectedResult = new BigInteger(testExpectation);

            // Act
            var subject = new BitString(hexValue);
            var result = subject.ToBigInteger();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        // MSB
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "05 DC")]
        [TestCase(int.MaxValue, "7F FF FF FF")]
        [TestCase(long.MaxValue, "7F FF FF FF FF FF FF FF")]
        //[Test]
        //// LSB
        //[TestCase(1, "01")]
        //[TestCase(10, "0A")]
        //[TestCase(15, "0F")]
        //[TestCase(1500, "DC 05")]
        //[TestCase(int.MaxValue, "FF FF FF 7F")]
        //[TestCase(long.MaxValue, "FF FF FF FF FF FF FF 7F")]
        public void ShouldCreateInstanceFromHexStringLong(long testExpectation, string hexValue)
        {
            // Arrange
            var expectedResult = new BigInteger(testExpectation);

            // Act
            var subject = new BitString(hexValue);
            var result = subject.ToBigInteger();

            // Assert
            Assert.AreEqual(expectedResult, result);
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
        //[Test]
        //// MSb
        //// less than one byte
        //[TestCase(new bool[] { true }, 1)]
        //[TestCase(new bool[] { true, false, false }, 4)]
        //[TestCase(new bool[] { true, false, false, false }, 8)]
        //[TestCase(new bool[] { true, false, false, false, true }, 17)]
        //// one byte
        //[TestCase(new bool[] { true, false, false, false, false, false, false, false }, 128)]
        //// one byte plus some bits
        //[TestCase(new bool[] { true, false, true, true, true, false, true, true, true, false, false }, 1500)]
        //// two bytes
        //[TestCase(new bool[] { false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, true }, 16449)]
        //[TestCase(new bool[] { true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false }, 32896)]
        //// three bytes
        //[TestCase(new bool[] { true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false, true, false, false, false, false, false, false, false }, 8421504)]
        [Test]
        // LSb
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

            var expectedBytes = BitConverter.GetBytes(expectedResult).Reverse().ToArray();
            int offsetDueToZeros = 0;
            for (int i = 0; i < expectedBytes.Length; i++)
            {
                if (expectedBytes[i] == 0)
                {
                    offsetDueToZeros = i;
                }
                else
                {
                    offsetDueToZeros++;
                    break;
                }
            }

            // Act
            var results = bitString.ToBytes();

            // Assert
            for (int i = 0; i < expectedBytes.Length - offsetDueToZeros; i++)
            {
                Assert.AreEqual(expectedBytes[i + offsetDueToZeros], results[i]);
            }
        }

        [Test]
        [TestCase(long.MinValue)]
        [TestCase(long.MinValue + 42)]
        [TestCase(long.MaxValue - 42)]
        [TestCase(long.MaxValue)]
        public void ToBytesLargerThanFourBytes(long largeNumber)
        {
            var bytes = BitConverter.GetBytes(largeNumber).Reverse().ToArray();

            BitString bitString = new BitString(bytes);

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

        //[Test]
        //[TestCase(1, new byte[] { 1 })]
        //[TestCase(255, new byte[] { 255, 0 })]
        //[TestCase(256, new byte[] { 0, 1 })]
        //[TestCase(1500, new byte[] { 220, 5 })]
        //public void ToBytesShouldBeInLeastSignificantByteOrder(int valueToToBytes, byte[] expectedByteArrayOrder)
        //{
        //    // Arrange
        //    BitString bs = new BitString(new BigInteger(valueToToBytes));

        //    // Act
        //    var subject = bs.ToBytes();

        //    // Assert
        //    for (int i = 0; i < expectedByteArrayOrder.Length; i++)
        //    {
        //        Assert.AreEqual(expectedByteArrayOrder[i], subject[i]);
        //    }
        //}

        [Test]
        [TestCase(1, new byte[] { 1 })]
        [TestCase(255, new byte[] { 0, 255 })]
        [TestCase(256, new byte[] { 1, 0 })]
        [TestCase(1500, new byte[] { 5, 220 })]
        public void ToBytesShouldBeInMostSignificantByteOrder(int valueToToBytes, byte[] expectedByteArrayOrder)
        {
            // Arrange
            BitString bs = new BitString(new BigInteger(valueToToBytes));

            // Act
            var subject = bs.ToBytes();

            // Assert
            for (int i = 0; i < expectedByteArrayOrder.Length; i++)
            {
                Assert.AreEqual(expectedByteArrayOrder[i], subject[i]);
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
            var bytes = BitConverter.GetBytes(toStringNumber).Reverse().ToArray();

            BitString bitString = new BitString(bytes);

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
            int length = bs.BitLength;

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
            int length = bs.BitLength;

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
            int length = bs.BitLength;

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
            new bool[] { false },
            new bool[] { true },
            new bool[] { true, false }
        )]
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
            for (int i = 0; i < results.BitLength; i++)
            {
                Assert.AreEqual(expectedResult[i], results.Bits[i]);
            }
        }

        [Test]
        [TestCase(
            new bool[] { false },
            new bool[] { true },
            new bool[] { true, false }
        )]
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
        public void ConcatenateBitsToExistingBits(bool[] leftSide, bool[] rightSide, bool[] expectedResult)
        {
            // Arrange
            BitString subject = new BitString(new BitArray(leftSide));
            BitString newBits = new BitString(new BitArray(rightSide));

            // Act
            BitString results = subject.ConcatenateBits(newBits);

            // Assert
            for (int i = 0; i < results.BitLength; i++)
            {
                Assert.AreEqual(expectedResult[i], results.Bits[i]);
            }
        }
        #endregion ConcatenateBits

        #region MostSignificant
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
        public void StaticMostSignificantShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = BitString.GetMostSignificantBits(numberOfBits, bs);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }

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
        public void MostSignificantShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = bs.GetMostSignificantBits(numberOfBits);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }
        #endregion MostSignificant

        #region LeastSignificant
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
        public void StaticLeastSignificantShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = BitString.GetLeastSignificantBits(numberOfBits, bs);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }

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
        public void LeastSignificantShouldReturnNumberOfDigitsSpecified(bool[] workingArray, int numberOfBits, bool[] expectedArray)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(workingArray));

            // Act
            var results = bs.GetLeastSignificantBits(numberOfBits);

            // Assert
            Assert.AreEqual(expectedArray, results.Bits);
        }
        #endregion LeastSignificant

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

        //[Test]
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
        [Test]
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
        public void ToBigIntegerIntReturnsBigIntegerBasedOnBytes(int testInt)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(testInt).Reverse().ToArray();

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
        public void ToBigIntegerLongReturnsBigIntegerBasedOnBytes(long testLong)
        {
            // Arrange
            var bytes = BitConverter.GetBytes(testLong).Reverse().ToArray();

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
        [TestCase("01")]
        [TestCase("0A")]
        [TestCase("0F")]
        [TestCase("05DC")]
        [TestCase("7FFFFFFF")]
        public void ToHexShouldReturnSameHexStringFromConstructor(string expectedHexString)
        {
            var bs = new BitString(expectedHexString);
            var result = bs.ToHex();
            Assert.AreEqual(expectedHexString, result);
        }

        [Test]
        [TestCase(1, "01")]
        [TestCase(10, "0A")]
        [TestCase(15, "0F")]
        [TestCase(1500, "05DC")]
        [TestCase(int.MaxValue, "7FFFFFFF")]
        public void ToHexShouldReturnHexStringOfBitString(int testInt, string expectedHex)
        {
            // Arrange
            var testBigInt = new BigInteger(testInt);
            var bs = new BitString(testBigInt);

            // Act
            var results = bs.ToHex();

            // Assert
            // Note MSB
            Assert.AreEqual(expectedHex, results);
        }
        #endregion ToHex

        // TODO - does this method do anything outside of the scope of itself currently?
        #region ToDigit
        #endregion ToDigit

        #region GetHashCode
        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(1500)]
        [Ignore("Will fail for current implementation of BitString until a new GetHashCode() is implemented.")]
        public void GetHashCodeShouldBeConsistent(int testInt)
        {
            // Arrange
            var testBigInt = new BigInteger(testInt);
            var bs1 = new BitString(testBigInt);
            var bs2 = new BitString(testBigInt);

            // Act
            var firstHash = bs1.GetHashCode();
            var secondHash = bs2.GetHashCode();

            // Assert
            Assume.That(bs1.Equals(bs2));
            Assert.AreEqual(firstHash, secondHash);
        }

        // The logic here is a -> b
        // Equality of BitStrings corresponds to a
        // Matching GetHashCodes corresponds to b
        // If two BitStrings are equal, then their GetHashCodes must also be equal
        [Test]
        [TestCase(1)]
        [TestCase(10)]
        [TestCase(15)]
        [TestCase(1500)]
        public void EqualityShouldImplyMatchingGetHashCode(int testInt)
        {
            // Arrange
            var testBigInt = new BigInteger(testInt);
            var bs1 = new BitString(testBigInt);
            var bs2 = bs1;

            // Act
            var firstHash = bs1.GetHashCode();
            var secondHash = bs2.GetHashCode();

            // Assert
            Assume.That(bs1.Equals(bs2));
            Assert.AreEqual(firstHash, secondHash);
        }

        // Using the contrapositive, ~b -> ~a
        // Inequality of BitStrings corresponds to ~a
        // Mismatching GetHashCodes corresponds to ~b
        // If their GetHashCodes are unequal, then two BitStrings must be unequal
        [Test]
        [TestCase(1, 2)]
        [TestCase(10, 11)]
        [TestCase(15, 51)]
        [TestCase(1500, 1050)]
        public void MismatchingGetHashCodeShouldImplyInequality(int testInt1, int testInt2)
        {
            // Arange
            var testBigInt1 = new BigInteger(testInt1);
            var testBigInt2 = new BigInteger(testInt2);
            var bs1 = new BitString(testBigInt1);
            var bs2 = new BitString(testBigInt2);

            // Act
            var firstHash = bs1.GetHashCode();
            var secondHash = bs2.GetHashCode();

            // Assert
            Assume.That(firstHash != secondHash);
            Assert.AreNotEqual(bs1, bs2);
        }
        #endregion GetHexCode 
    }
}

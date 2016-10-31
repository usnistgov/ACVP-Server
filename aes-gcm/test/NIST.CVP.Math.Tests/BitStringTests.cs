using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests
{
    [TestFixture]
    public class BitStringTests
    {

        #region Equals
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

        #region ToString
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

        #region GetBytes
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
        public void GetBytesConvertsBinaryToBytes(bool[] bits, int expectedResult)
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

        [TestCase(long.MinValue)]
        [TestCase(long.MinValue + 42)]
        [TestCase(long.MaxValue - 42)]
        [TestCase(long.MaxValue)]
        public void GetBytesLargerThanFourBytes(long largeNumber)
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
        public void GetBytesReturnsBytesInReverseOrderWhenSpecified(bool[] bits)
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
        #endregion GetBytes

        #region ConcatenateBits
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

        [TestCase(1)]
        [TestCase(2)]
        public void StaticSubStringThrowsArgumentOutOfRangeExceptionWhenStartIndexGreaterThanBitStringLength(int startIndex)
        {
            // Arrange
            BitString bs = new BitString(new BitArray(new bool[] { true }));

            // Act / Assert
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => BitString.Substring(bs, startIndex, 1));
        }

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

        [TestCase(
                    new bool[] { true, true },
                    new bool[] { true },
                    new bool[] { true, false }
                )]
        [TestCase(
                    new bool[] { true, false },
                    new bool[] { true },
                    new bool[] { true, true }
                )]
        [TestCase(
                    new bool[] { false, true },
                    new bool[] { true },
                    new bool[] { false, false }
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

        [TestCase(
            new bool[] { true, true },
            new bool[] { true },
            new bool[] { true, false }
        )]
        [TestCase(
            new bool[] { true, false },
            new bool[] { true },
            new bool[] { true, true }
        )]
        [TestCase(
            new bool[] { false, true },
            new bool[] { true },
            new bool[] { false, false }
        )]
        [TestCase(
            new bool[] { true },
            new bool[] { true, false },
            new bool[] { true, true }
        )]
        [TestCase(
            new bool[] { true, false, true, false, true },
            new bool[] { false, true, true },
            new bool[] { true, false, true, true, false }
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


    }
}

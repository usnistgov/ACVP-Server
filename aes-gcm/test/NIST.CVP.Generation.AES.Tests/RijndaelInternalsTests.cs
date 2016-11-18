using Moq;
using NIST.CVP.Math;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.AES.Tests
{
    /// <summary>
    /// Note unit tests can utilize <see cref="RijndaelTest"/> for hitting protected functions.
    /// </summary>
    [TestFixture]
    public class RijndaelInternalsTests
    {
        /// <summary>
        /// Creates a 2d byte array of values.  Given default values of 4 and 8, array will look like:
        /// 
        ///     {   0,  1,  2,  3,  4,  5,  6,  7 },
        ///     {   8,  9,  10, 11, 12, 13, 14, 15 },
        ///     {   16, 17, 18, 19, 20, 21, 22, 23 },
        ///     {   24, 25, 26, 27, 28, 29, 30, 31 }
        /// 
        /// </summary>
        /// <param name="dimension1Count">Dimension 1 size</param>
        /// <param name="dimension2Count">Dimension 2 size</param>
        /// <returns></returns>
        private byte[,] GetTestBlock(int dimension1Count = 4, int dimension2Count = 8)
        {
            byte[,] result = new byte[dimension1Count, dimension2Count];

            byte count = 0;
            for (int d1iterator = 0; d1iterator < dimension1Count; d1iterator++)
            {
                for (int d2iterator = 0; d2iterator < dimension2Count; d2iterator++)
                {
                    result[d1iterator, d2iterator] = count;
                    count++;
                }
            }

            return result;
        }

        [Test]
        public void ShouldContainExpectedTestData()
        {
            Array2D testData = new Array2D(GetTestBlock());
            Assert.AreEqual(4, testData.Dimension1Size, nameof(testData.Dimension1Size));
            Assert.AreEqual(8, testData.Dimension2Size, nameof(testData.Dimension2Size));

            byte count = 0;
            for (int dimension1 = 0; dimension1 < testData.Dimension1Size; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < testData.Dimension2Size; dimension2++)
                {
                    Assert.AreEqual(count, testData.Array[dimension1, dimension2]);
                    count++;
                }
            }
        }

        #region EncryptSingleBlock
        // @@@ TODO
        #endregion EncryptSingleBlock

        #region KeyAddition
        // @@@ TODO
        #endregion KeyAddition

        #region Substitution
        // @@@ TODO
        #endregion Substitution

        #region ShiftRow
        [Test]
        public void ShouldNotModifyRow0ButOtherRowsAreModifiedWithShiftRow()
        {
            RijndaelInternals sut = new RijndaelInternals();
            var testBlock = GetTestBlock();

            byte[] row0 = new byte[8];
            for (int i = 0; i < 8; i++)
            {
                row0[i] = testBlock[0, i];
            }

            byte[,] otherRows = new byte[3, 8];
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    otherRows[dimension1, dimension2] = testBlock[dimension1 + 1, dimension2];
                }
            }

            sut.ShiftRow(testBlock, 0, 8);

            // Check row 0 has not changed
            for (int i = 0; i < 8; i++)
            {
                Assert.AreEqual(testBlock[0,i], row0[i], "Equal check");
            }

            // Check other rows have changed
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    Assert.AreNotEqual(testBlock[dimension1 + 1, dimension2], otherRows[dimension1, dimension2], "Not equal check");
                }
            }
        }

        [Test]
        public void ShouldKeepValuesWithinRow()
        {
            RijndaelInternals sut = new RijndaelInternals();
            var testBlock = GetTestBlock();

            byte[,] rowsOverIndex0 = new byte[3, 8];
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    rowsOverIndex0[dimension1, dimension2] = testBlock[dimension1 + 1, dimension2];
                }
            }

            sut.ShiftRow(testBlock, 0, 8);

            // Ensure values within row are values only from the original values within that row
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                List<byte> bytesInOriginalRow = new List<byte>();
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    bytesInOriginalRow.Add(rowsOverIndex0[dimension1, dimension2]);
                }
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    Assert.IsTrue(bytesInOriginalRow.Contains(testBlock[dimension1 + 1, dimension2]));
                }
                bytesInOriginalRow.Clear();
            }
        }
        #endregion ShiftRow

        #region ShouldMixColumns
        /// <summary>
        /// Given a 2d array[4,8]:
        /// 
        ///     {   0,  1,  2,  3,  4,  5,  6,  7 },
        ///     {   8,  9,  10, 11, 12, 13, 14, 15 },
        ///     {   16, 17, 18, 19, 20, 21, 22, 23 },
        ///     {   24, 25, 26, 27, 28, 29, 30, 31 }
        /// 
        /// Supplying 0 for both runs of Multiply, gives for each iteration of [i,j]
        /// 
        ///     0 ^ 0 ^ block[(i + 2) % 4, j] ^ block[(i + 3) % 4, j]
        ///     
        /// So for iteration on block[2, 3] = 19, plugging in values we get: 
        ///     0 ^ 0 ^ block[(2 + 2) % 4, 3] ^ block[(2 + 3) % 4, 3]
        ///  or 0 ^ 0 ^ block[0, 3] ^ block[1, 3]
        ///  or 0 ^ 0 ^ 3 ^ 11
        ///  or 00000000 ^ 00000000 ^ 00000011 ^ 00001011
        /// 
        ///     00000000 ^
        ///     00000000 ^
        ///     00000011 ^ 
        ///     00001011
        ///     --------
        ///     00001000 = 8
        ///     
        ///     block[2, 3] which was 19, should become 8.
        /// </summary>
        [Test]
        // See comments above for breakdown
        [TestCase(0, 0, 2, 3, 19, 8)]
        /*
         * block[2,3] = 19
         * block[(2+2)%4, 3] = block[0, 3] = 3
         * block[(2+3)%4, 3] = block[1, 3] = 11
         * 
         *      224     11100000
         *       80     01010000
         *        3     00000011
         *       11     00001011
         *       ---------------
         *              10111000 = 184
         */
        [TestCase(224, 80, 2, 3, 19, 184)]
        /*
         * block[2,3] = 19
         * block[(2+2)%4, 3] = block[0, 3] = 3
         * block[(2+3)%4, 3] = block[1, 3] = 11
         * 
         *      255     11111111
         *       80     01010000
         *        3     00000011
         *       11     00001011
         *       ---------------
         *              10100111 = 167
         */
        [TestCase(255, 80, 2, 3, 19, 167)]
        /*
         * block[0,0] = 0
         * block[(0+2)%4, 0] = block[2, 0] = 16
         * block[(0+3)%4, 0] = block[3, 0] = 24
         * 
         *      255     11111111
         *       80     01010000
         *       16     00010000
         *       24     00011000
         *       ---------------
         *              10100111 = 167
         */
        [TestCase(255, 80, 0, 0, 0, 167)]
        public void ShouldMixColumns(byte multiplyFirstReturn, byte multiplySecondReturn, byte row, byte column, byte rowColumnOriginal, byte rowColumnExpectation)
        {
            Mock<RijndaelInternals> sut = new Mock<RijndaelInternals>();
            sut.CallBase = true;
            sut
                .Setup(s => s.Multiply(2, It.IsAny<byte>()))
                .Returns(multiplyFirstReturn);
            sut
                .Setup(s => s.Multiply(3, It.IsAny<byte>()))
                .Returns(multiplySecondReturn);

            var testBlock = GetTestBlock();
            int blockCount = 8;

            Assume.That(testBlock[row, column] == rowColumnOriginal);

            sut.Object.MixColumn(testBlock, blockCount);

            Assert.AreEqual(rowColumnExpectation, testBlock[row, column]);
        }
        #endregion ShouldMixColumns

        #region Multiply
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        public void MultiplyShouldReturnZeroIfEitherAorBAre0(byte a, byte b)
        {
            RijndaelInternals sut = new RijndaelInternals();
            var result = sut.Multiply(a, b);
            Assert.AreEqual(0, result);
        }

        [Test]
        [TestCase(1, 1, 0, 0, 0, 1)]
        [TestCase(16, 32, 100, 125, 225, 54)]
        [TestCase(111, 112, 186, 43, 229, 123)]
        [TestCase(1, 255, 0, 7, 7, 255)]
        public void MultiplyShouldReturnByteFromAlgotableWhoseIndexIsTheIndexOfLogTableByteAPlusLogTableByteBMod255(
            byte a,
            byte b,
            byte logTableAValue,
            byte logTableBValue,
            byte expectedAlgoTableIndex,
            byte expectedAlgoTableValue
        )
        {
            RijndaelInternals sut = new RijndaelInternals();

            Assume.That(RijndaelBoxes.Logtable[a] == logTableAValue, nameof(logTableAValue));
            Assume.That(RijndaelBoxes.Logtable[b] == logTableBValue, nameof(logTableBValue));
            Assume.That(expectedAlgoTableIndex == (RijndaelBoxes.Logtable[a] + RijndaelBoxes.Logtable[b] % 255), nameof(expectedAlgoTableIndex));

            var result = sut.Multiply(a, b);
            Assert.AreEqual(expectedAlgoTableValue, result, nameof(expectedAlgoTableValue));
        }
        #endregion Multiply
    }
}

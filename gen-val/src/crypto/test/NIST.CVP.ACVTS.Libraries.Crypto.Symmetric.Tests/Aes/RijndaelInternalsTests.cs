﻿using System.Collections.Generic;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Tests.Aes
{
    /// <summary>
    /// Note unit tests can utilize <see cref="RijndaelTest"/> for hitting protected functions.
    /// </summary>
    [TestFixture, FastCryptoTest]
    public class RijndaelInternalsTests
    {
        /// <summary>
        /// publicly exposing protected functions to get at RijndaelInternals
        /// </summary>
        public class TestableAesEngine : AesEngine
        {
            public new byte[,] PopulateMultiDimensionBlock(byte[] payLoad, int blockIndex)
            {
                return base.PopulateMultiDimensionBlock(payLoad, blockIndex);
            }

            public new void PopulateOutputFromResult(byte[,] multiDimensionBlock, byte[] outBuffer, int blockIndex)
            {
                base.PopulateOutputFromResult(multiDimensionBlock, outBuffer, blockIndex);
            }
        }

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
        private static byte[,] GetTestBlock(int dimension1Count = 4, int dimension2Count = 8)
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
            Assert.That(testData.Dimension1Size, Is.EqualTo(4), nameof(testData.Dimension1Size));
            Assert.That(testData.Dimension2Size, Is.EqualTo(8), nameof(testData.Dimension2Size));

            byte count = 0;
            for (int dimension1 = 0; dimension1 < testData.Dimension1Size; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < testData.Dimension2Size; dimension2++)
                {
                    Assert.That(testData.Array[dimension1, dimension2], Is.EqualTo(count));
                    count++;
                }
            }
        }

        #region EncryptSingleBlock
        static object[] encryptSingleBlockTestCase = new object[]
        {
            new object[]
            {
                "test1",
                GetTestBlock(),
                new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
                BlockCipherDirections.Encrypt
            },
            new object[]
            {
                "test2",
                GetTestBlock(),
                new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 },
                BlockCipherDirections.Decrypt
            }
        };
        [Test]
        [TestCaseSource(nameof(encryptSingleBlockTestCase))]
        public void EncryptSingleBlockShouldRunInternalMethodsMultipleTimesAtLeastOnePerRound(string label, byte[,] block, byte[] key, BlockCipherDirections direction)
        {
            Mock<TestableAesEngine> subject = new Mock<TestableAesEngine>
            {
                CallBase = true
            };

            var payload = new byte[subject.Object.BlockSizeBytes];
            subject.Object.PopulateOutputFromResult(block, payload, 0);
            subject.Object.Init(new EngineInitParameters(direction, key));
            var keySchedule = subject.Object.PopulateKeySchedule();
            subject.Object.ProcessSingleBlock(payload, new byte[subject.Object.BlockSizeBytes], 0);

            // KeyAddition runs 2x + 1x for each round within key - 1
            int expectedKeyAdditionRounds = 2 + keySchedule.Rounds - 1;
            // Substitution runs 1x + 1x for each round within key - 1
            int expectedSubstitutionRounds = 1 + keySchedule.Rounds - 1;
            // ShiftRow runs 1x + 1x for each round within key - 1
            int expectedShiftRow = 1 + keySchedule.Rounds - 1;
            // MixColumn runs 1x for each round within key - 1, encrypt only
            int expectedMixColumns = direction == BlockCipherDirections.Encrypt ? keySchedule.Rounds - 1 : 0;
            // InvMixColumn runs 1x for each round within key - 1, decrypt only
            int expectedInvMixColumns = direction == BlockCipherDirections.Decrypt ? keySchedule.Rounds - 1 : 0;

            subject.Verify(v => v.KeyAddition(It.IsAny<byte[,]>(), It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Exactly(expectedKeyAdditionRounds),
                nameof(subject.Object.KeyAddition));
            subject.Verify(v => v.Substitution(It.IsAny<byte[,]>(), It.IsAny<byte[]>(), It.IsAny<int>()),
                Times.Exactly(expectedSubstitutionRounds),
                nameof(subject.Object.Substitution));
            subject.Verify(v => v.ShiftRow(It.IsAny<byte[,]>(), It.IsAny<int>(), It.IsAny<int>()),
                Times.Exactly(expectedShiftRow),
                nameof(subject.Object.ShiftRow));
            subject.Verify(v => v.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Exactly(expectedMixColumns),
                nameof(subject.Object.MixColumn));
            subject.Verify(v => v.InvMixColumn(It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Exactly(expectedInvMixColumns),
                nameof(subject.Object.InvMixColumn));
        }
        #endregion EncryptSingleBlock

        #region KeyAddition
        static object[] keyAdditionTestCase = new object[]
        {
            new object[]
            {
                "test1",
                new byte[4,1] { { 1 }, { 7 }, { 9 }, { 10 } },
                new byte[4,1] { { 1 }, { 7 }, { 9 }, { 10 } },
                new byte[4,1] { { 0 }, { 0 }, { 0 }, { 0 } }
            },
            new object[]
            {
                "test2",
                new byte[4,1]
                {
                    { 1 },      // 00000001
                    { 7 },      // 00000111
                    { 9 },      // 00001001
                    { 10 }      // 00001010
                },
                new byte[4,1]
                {
                    { 123 },    // 01111011
                    { 42 },     // 00101010
                    { 55 },     // 00110111
                    { 250 }     // 11111010
                },
                new byte[4,1]
                { 
                    // 00000001 (1)
                    // 01111011 (123)
                    // --------
                    // 01111010 (122)
                    { 122 }, 
                    // 00000111 (7)
                    // 00101010 (42)
                    // --------
                    // 00101101 (45)
                    { 45 }, 
                    // 00001001 (9)
                    // 00110111 (55)
                    // --------
                    // 00111110 (62)
                    { 62 }, 
                    // 00001010 (10)
                    // 11111010 (250)
                    // --------
                    // 11110000 (240)
                    { 240 }
                }
            },
        };
        /// <summary>
        /// Implementation takes in a <see cref="RijndaelKeySchedule.Schedule"/>, 
        /// using a fake schedule for testing purposes.
        /// 
        /// Should XOR the block[i,j] with the corresponding Schedule[i,j]
        /// </summary>
        [Test]
        [TestCaseSource(nameof(keyAdditionTestCase))]
        public void ShouldXorBlockWithProvidedBlockForKeyAddition(string label, byte[,] block, byte[,] schedule, byte[,] expectedBlock)
        {
            TestableAesEngine subject = new TestableAesEngine();
            Array2D array = new Array2D(block);

            subject.KeyAddition(array.Array, schedule, array.Dimension2Size);

            for (int dimension1 = 0; dimension1 < array.Dimension1Size; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < array.Dimension2Size; dimension2++)
                {
                    Assert.That(array.Array[dimension1, dimension2], Is.EqualTo(expectedBlock[dimension1, dimension2]));
                }
            }
        }
        #endregion KeyAddition

        #region Substitution
        static object[] substituteTestCase = new object[]
        {
            new object[]
            {
                "test1",
                new byte[4,1] { { 1 }, { 7 }, { 9 }, { 10 } },
                new byte[] { 100, 99, 98, 97, 96, 95, 94, 93, 92, 91, 90 },
                new byte[4,1] { { 99 }, { 93 }, { 91 }, { 90 } }
            },
            new object[]
            {
                "test2",
                new byte[4,1] { { 1 }, { 2 }, { 3 }, { 10 } },
                new byte[] { 255, 1, 55, 99, 100, 77, 66, 11, 13, 44, 59 },
                new byte[4,1] { { 1 }, { 55 }, { 99 }, { 59 } }
            },
        };
        /// <summary>
        /// Note in implementation, <see cref="RijndaelBoxes.S"/> is used rather the passed test sBox
        /// 
        /// for each value in block[i,j]
        ///     - Find the index within the sBox that corresponds to the value of block[i,j]
        ///     - Replace block[i,j] with the value found at the index of sBox
        ///     
        /// </summary>
        /// <param name="block"></param>
        /// <param name="sBox"></param>
        /// <param name="expectedBlock"></param>
        [Test]
        [TestCaseSource(nameof(substituteTestCase))]
        public void ShouldSubstituteFromProvidedSBox(string label, byte[,] block, byte[] sBox, byte[,] expectedBlock)
        {
            TestableAesEngine subject = new TestableAesEngine();
            Array2D array = new Array2D(block);

            subject.Substitution(array.Array, sBox, array.Dimension2Size);

            for (int dimension1 = 0; dimension1 < array.Dimension1Size; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < array.Dimension2Size; dimension2++)
                {
                    Assert.That(array.Array[dimension1, dimension2], Is.EqualTo(expectedBlock[dimension1, dimension2]));
                }
            }
        }
        #endregion Substitution

        #region ShiftRow
        [Test]
        public void ShouldNotModifyRow0ButOtherRowsAreModifiedWithShiftRow()
        {
            TestableAesEngine subject = new TestableAesEngine();
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

            subject.ShiftRow(testBlock, 0, 8);

            // Check row 0 has not changed
            for (int i = 0; i < 8; i++)
            {
                Assert.That(row0[i], Is.EqualTo(testBlock[0, i]), "Equal check");
            }

            // Check other rows have changed
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    Assert.That(otherRows[dimension1, dimension2], Is.Not.EqualTo(testBlock[dimension1 + 1, dimension2]), "Not equal check");
                }
            }
        }

        [Test]
        public void ShouldKeepValuesWithinRow()
        {
            TestableAesEngine subject = new TestableAesEngine();
            var testBlock = GetTestBlock();

            byte[,] rowsOverIndex0 = new byte[3, 8];
            for (int dimension1 = 0; dimension1 < 3; dimension1++)
            {
                for (int dimension2 = 0; dimension2 < 8; dimension2++)
                {
                    rowsOverIndex0[dimension1, dimension2] = testBlock[dimension1 + 1, dimension2];
                }
            }

            subject.ShiftRow(testBlock, 0, 8);

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
                    Assert.That(bytesInOriginalRow.Contains(testBlock[dimension1 + 1, dimension2]), Is.True);
                }
                bytesInOriginalRow.Clear();
            }
        }
        #endregion ShiftRow

        #region MixColumns
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
        /// <param name="multiplyFirstReturn">THe value the call to the first multiply should return</param>
        /// <param name="multiplySecondReturn">THe value the call to the second multiply should return</param>
        /// <param name="row">The row to use from the test block</param>
        /// <param name="column">The column to use from the test block</param>
        /// <param name="rowColumnOriginal">The original value of the row/column combination</param>
        /// <param name="rowColumnExpectation">The expected value of the row/column combination</param>
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
        public void ShouldMixColumns(byte multiplyFirstReturn, byte multiplySecondReturn, byte row, byte column,
            byte rowColumnOriginal, byte rowColumnExpectation)
        {
            Mock<TestableAesEngine> subject = new Mock<TestableAesEngine>();
            subject.CallBase = true;
            subject
                .Setup(s => s.Multiply(2, It.IsAny<byte>()))
                .Returns(multiplyFirstReturn);
            subject
                .Setup(s => s.Multiply(3, It.IsAny<byte>()))
                .Returns(multiplySecondReturn);

            var testBlock = GetTestBlock();
            int blockCount = 8;

            Assert.That(testBlock[row, column] == rowColumnOriginal);

            subject.Object.MixColumn(testBlock, blockCount);

            Assert.That(testBlock[row, column], Is.EqualTo(rowColumnExpectation));
        }

        /// <summary>
        /// Tests the inverse mix column function, similar to tests in <see cref="ShouldMixColumns"/>
        /// </summary>
        /// <param name="multiplyFirstReturn">THe value the call to the first multiply should return</param>
        /// <param name="multiplySecondReturn">THe value the call to the second multiply should return</param>
        /// <param name="multiplyThirdReturn">THe value the call to the third multiply should return</param>
        /// <param name="multiplyFourthReturn">THe value the call to the fourth multiply should return</param>
        /// <param name="row">The row to use from the test block</param>
        /// <param name="column">The column to use from the test block</param>
        /// <param name="rowColumnOriginal">The original value of the row/column combination</param>
        /// <param name="rowColumnExpectation">The expected value of the row/column combination</param>
        [Test]
        //  0       00000000 ^
        //  0       00000000 ^
        //  0       00000000 ^
        //  255     11111111
        //          --------
        //  expect  11111111
        [TestCase(
            0, // multiplyFirstReturn
            0, // multiplySecondReturn
            0, // multiplyThirdReturn
            255, // multiplyFourthReturn
            0, // row index from test block
            0, // column index from test block
            0, // testBlock[row, column] assumption value
            255 // testBlock[row, column] expectation value after mixing
        )]
        //  0       00000000 ^
        //  64      01000000 ^
        //  171     10101011 ^
        //  98      01100010
        //          --------
        //  expect  10001001    137
        [TestCase(
            0, // multiplyFirstReturn
            64, // multiplySecondReturn
            171, // multiplyThirdReturn
            98, // multiplyFourthReturn
            2, // row index from test block
            4, // column index from test block
            20, // testBlock[row, column] assumption value
            137 // testBlock[row, column] expectation value after mixing
        )]
        public void ShouldInvMixColumns(byte multiplyFirstReturn, byte multiplySecondReturn,
            byte multiplyThirdReturn, byte multiplyFourthReturn,
            byte row, byte column, byte rowColumnOriginal, byte rowColumnExpectation)
        {
            Mock<TestableAesEngine> subject = new Mock<TestableAesEngine>();
            subject.CallBase = true;
            subject
                .Setup(s => s.Multiply(14, It.IsAny<byte>()))
                .Returns(multiplyFirstReturn);
            subject
                .Setup(s => s.Multiply(11, It.IsAny<byte>()))
                .Returns(multiplySecondReturn);
            subject
                .Setup(s => s.Multiply(13, It.IsAny<byte>()))
                .Returns(multiplyThirdReturn);
            subject
                .Setup(s => s.Multiply(9, It.IsAny<byte>()))
                .Returns(multiplyFourthReturn);

            var testBlock = GetTestBlock();
            int blockCount = 8;

            Assert.That(testBlock[row, column] == rowColumnOriginal);

            subject.Object.InvMixColumn(testBlock, blockCount);

            Assert.That(testBlock[row, column], Is.EqualTo(rowColumnExpectation));
        }

        [Test]
        public void ShouldCallMultiply2xForEachBlockIndex()
        {
            Mock<TestableAesEngine> subject = new Mock<TestableAesEngine>();
            subject.CallBase = true;

            var testBlock = GetTestBlock();
            int blockCount = 8;

            int runsPerBlockIndex = 2;
            int totalBlockIndeces = 4 * 8;
            int totalRuns = runsPerBlockIndex * totalBlockIndeces;

            subject.Object.MixColumn(testBlock, blockCount);

            subject.Verify(v => v.MixColumn(It.IsAny<byte[,]>(), It.IsAny<int>()),
                Times.Once,
                nameof(subject.Object.MixColumn));
            subject.Verify(v => v.Multiply(It.IsAny<byte>(), It.IsAny<byte>()),
                Times.Exactly(totalRuns),
                nameof(subject.Object.Multiply));
        }
        #endregion MixColumns

        #region Multiply
        [Test]
        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(0, 0)]
        public void MultiplyShouldReturnZeroIfEitherAorBAre0(byte a, byte b)
        {
            TestableAesEngine subject = new TestableAesEngine();
            var result = subject.Multiply(a, b);
            Assert.That(result, Is.EqualTo(0));
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
            TestableAesEngine subject = new TestableAesEngine();

            Assert.That(RijndaelBoxes.Logtable[a] == logTableAValue, nameof(logTableAValue));
            Assert.That(RijndaelBoxes.Logtable[b] == logTableBValue, nameof(logTableBValue));
            Assert.That(expectedAlgoTableIndex == (RijndaelBoxes.Logtable[a] + RijndaelBoxes.Logtable[b] % 255), nameof(expectedAlgoTableIndex));

            var result = subject.Multiply(a, b);
            Assert.That(result, Is.EqualTo(expectedAlgoTableValue), nameof(expectedAlgoTableValue));
        }
        #endregion Multiply
    }
}

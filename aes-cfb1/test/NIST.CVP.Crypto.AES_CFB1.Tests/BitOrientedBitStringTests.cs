using System;
using System.Collections;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Crypto.AES_CFB1.Tests
{
    [TestFixture, UnitTest]
    public class BitOrientedBitStringTests
    {

        #region Derived
        [Test]
        [TestCase(typeof(BitString), true)]
        [TestCase(typeof(BitOrientedBitString), false)]
        public void DerivedTypeShouldNotEvaluateToOriginalType(Type originalType, bool isTypeOf)
        {
            var result = originalType == typeof(BitString); 

            Assert.AreEqual(isTypeOf, result);
        }

        [Test]
        [TestCase(typeof(BitString), false)]
        [TestCase(typeof(BitOrientedBitString), true)]
        public void OriginalTypeShouldNotEvaluateToDerivedType(Type originalType, bool isTypeOf)
        {
            var result = originalType == typeof(BitOrientedBitString);

            Assert.AreEqual(isTypeOf, result);
        }

        [Test]
        public void DerivedTypeShouldTransformProperly()
        {
            BitString bs = new BitString(5);

            var result = BitOrientedBitString.GetDerivedFromBase(bs);

            Assert.AreEqual(bs.Bits, result.Bits);
        }

        [Test]
        public void DerivedTypeShouldReturnNullWhenOriginalIsNull()
        {
            var result = BitOrientedBitString.GetDerivedFromBase(null);

            Assert.IsNull(result);
        }
        #endregion Derived

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

            BitOrientedBitString workingBs = new BitOrientedBitString(workingBitArray);
            BitOrientedBitString compareBs = new BitOrientedBitString(compareBitArray);

            // Act
            var results = workingBs.Equals(compareBs);

            // Assert
            Assert.IsTrue(results);
        }

        [Test]
        public void EqualsMethodReturnsFalseWhenCompareObjectIsntAppropriateType()
        {
            // Arrange
            BitOrientedBitString bs = new BitOrientedBitString(new BitArray(new bool[] { true }));
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

            BitOrientedBitString workingBs = new BitOrientedBitString(workingBitArray);
            BitOrientedBitString compareBs = new BitOrientedBitString(compareBitArray);

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

            BitOrientedBitString workingBs = new BitOrientedBitString(workingBitArray);
            BitOrientedBitString compareBs = new BitOrientedBitString(compareBitArray);

            // Act
            var results = workingBs.Equals(compareBs);

            // Assert
            Assert.IsFalse(results);
        }
        #endregion Equals

        [Test]
        [TestCase("10")]
        [TestCase("1000")]
        public void ShouldReverse1sAnd0sInCreatingABitOrientedBitString(string input)
        {
            var subject = BitOrientedBitString.GetBitStringEachCharacterOfInputIsBit(input);

            // Note that bits input are MSb and ToString prints in MSb
            Assert.AreEqual(input, subject.ToString());
        }
    }
}

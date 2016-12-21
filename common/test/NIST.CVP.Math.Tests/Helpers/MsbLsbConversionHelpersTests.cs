using NIST.CVP.Math.Helpers;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Math.Tests.Helpers
{
    [TestFixture]
    public class MsbLsbConversionHelpersTests
    {
        [Test]
        [TestCase(new byte[] { 1 })]
        [TestCase(new byte[] { 1, 2, 3 })]
        [TestCase(new byte[] { 1, 2, 3, 4, 5 })]
        public void ReverseByteOrderReturnsBytesInReverseOrder(byte[] bytes)
        {
            var expectedResults = bytes.Reverse().ToArray();
            var subject = MsbLsbConversionHelpers.ReverseByteOrder(bytes);

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(expectedResults[i], subject[i]);
            }
        }

        [Test]
        [TestCase(new bool[] { })]
        [TestCase(new bool[] { true })]
        [TestCase(new bool[] { true, false })]
        [TestCase(new bool[] { true, false, false })]
        [TestCase(new bool[] { true, false, false, false, false, false, false, false, false, false })]
        public void ReverseBitArrayBitsShouldReturnBitArrayInReverseOrder(bool[] bits)
        {
            var ba = new BitArray(bits);
            var subject = MsbLsbConversionHelpers.ReverseBitArrayBits(ba);
            for (int i = 0; i < subject.Length; i++)
            {
                Assert.AreEqual(ba[i], subject[subject.Length - 1 - i]);
            }
        }

        [Test]
        [TestCase(new byte[] { 1 }, new bool[] { true, false, false, false, false, false, false, false })]
        [TestCase(new byte[] { 9 }, new bool[] { true, false, false, true, false, false, false, false })]
        [TestCase(new byte[] { 5, 220 }, new bool[] { false, false, true, true, true, false, true, true, true, false, true, false, false, false, false, false })]
        public void ShouldReturnLeastSignificantBitArrayFromMostSignificantByteArray(byte[] msBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.MostSignificantByteArrayToLeastSignificantBitArray(msBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(expectedLeastSignificantBits[i], result[i]);
            }
        }

        [Test]
        [TestCase(new byte[] { 1 }, new bool[] { false, false, false, false, false, false, false, true })]
        [TestCase(new byte[] { 9 }, new bool[] { false, false, false, false, true, false, false, true })]
        [TestCase(new byte[] { 5, 220 }, new bool[] { false, false, false, false, false, true, false, true, true, true, false, true, true, true, false, false })]
        public void ShouldReturnMostSignificantBitArrayFromMostSignificantByteArray(byte[] msBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.MostSignificantByteArrayToMostSignificantBitArray(msBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(expectedLeastSignificantBits[i], result[i]);
            }
        }

        [Test]
        [TestCase(new byte[] { 1 }, new bool[] { true, false, false, false, false, false, false, false })]
        [TestCase(new byte[] { 9 }, new bool[] { true, false, false, true, false, false, false, false })]
        [TestCase(new byte[] { 220, 5 }, new bool[] { false, false, true, true, true, false, true, true, true, false, true, false, false, false, false, false })]
        public void ShouldReturnLeastSignificantBitArrayFromLeastSignificantByteArray(byte[] lsBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.LeastSignificantByteArrayToLeastSignificantBitArray(lsBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(expectedLeastSignificantBits[i], result[i]);
            }
        }

        [Test]
        [TestCase(new byte[] { 1 }, new bool[] { false, false, false, false, false, false, false, true })]
        [TestCase(new byte[] { 9 }, new bool[] { false, false, false, false, true, false, false, true })]
        [TestCase(new byte[] { 220, 5 }, new bool[] { false, false, false, false, false, true, false, true, true, true, false, true, true, true, false, false })]
        public void ShouldReturnMostSignificantBitArrayFromLeastSignificantByteArray(byte[] lsBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.LeastSignificantByteArrayToMostSignificantBitArray(lsBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.AreEqual(expectedLeastSignificantBits[i], result[i]);
            }
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        public void ShouldThrowArgumentNullExceptionWhenStringNullOrEmpty(string testString)
        {
            Assert.Throws(
                typeof(ArgumentNullException), 
                () => MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(testString));
        }

        [Test]
        [TestCase("10a")]
        [TestCase("10f110")]
        [TestCase("100002110")]
        [TestCase("10000_110")]
        public void ShouldThrowArgumentExceptionWhenStringContainsCharactersBesidesSpaceZeroAndOne(string testString)
        {
            Assert.Throws(
                typeof(ArgumentException),
                () => MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(testString));
        }

        /// <summary>
        /// Note this only works for up to 8 characters as a "space" is added for each byte
        /// </summary>
        /// <param name="testString">The string to test with</param>
        [Test]
        [TestCase("1000")]
        [TestCase("1100")]
        [TestCase("11111110")]
        public void ShouldReturnBitStringInCorrectOrderLt8Characters(string testString)
        {
            var expectation = testString.Reverse().ToArray();

            var result = MsbLsbConversionHelpers.GetBitArrayFromStringOf1sAnd0s(testString);
            var bs = new BitString(result);

            Assert.AreEqual(expectation, bs.ToString());
        }
    }
}

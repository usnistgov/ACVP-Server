using System;
using System.Collections;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Math.Helpers;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Helpers
{
    [TestFixture, UnitTest]
    public class MsbLsbConversionHelpersTests
    {
        [Test]
        [TestCase("test1", new byte[] { 1 })]
        [TestCase("test2", new byte[] { 1, 2, 3 })]
        [TestCase("test3", new byte[] { 1, 2, 3, 4, 5 })]
        public void ReverseByteOrderReturnsBytesInReverseOrder(string label, byte[] bytes)
        {
            var expectedResults = bytes.Reverse().ToArray();
            var subject = MsbLsbConversionHelpers.ReverseByteOrder(bytes);

            for (int i = 0; i < bytes.Length; i++)
            {
                Assert.That(subject[i], Is.EqualTo(expectedResults[i]));
            }
        }

        [Test]
        [TestCase("test1", new bool[] { })]
        [TestCase("test2", new[] { true })]
        [TestCase("test3", new[] { true, false })]
        [TestCase("test4", new[] { true, false, false })]
        [TestCase("test5", new[] { true, false, false, false, false, false, false, false, false, false })]
        public void ReverseBitArrayBitsShouldReturnBitArrayInReverseOrder(string label, bool[] bits)
        {
            var ba = new BitArray(bits);
            var subject = MsbLsbConversionHelpers.ReverseBitArrayBits(ba);
            for (int i = 0; i < subject.Length; i++)
            {
                Assert.That(subject[subject.Length - 1 - i], Is.EqualTo(ba[i]));
            }
        }

        [Test]
        [TestCase("test1", new byte[] { 1 }, new[] { true, false, false, false, false, false, false, false })]
        [TestCase("test2", new byte[] { 9 }, new[] { true, false, false, true, false, false, false, false })]
        [TestCase("test3", new byte[] { 5, 220 }, new[] { false, false, true, true, true, false, true, true, true, false, true, false, false, false, false, false })]
        public void ShouldReturnLeastSignificantBitArrayFromMostSignificantByteArray(string label, byte[] msBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.MostSignificantByteArrayToLeastSignificantBitArray(msBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(expectedLeastSignificantBits[i]));
            }
        }

        [Test]
        [TestCase("test1", new byte[] { 1 }, new[] { false, false, false, false, false, false, false, true })]
        [TestCase("test2", new byte[] { 9 }, new[] { false, false, false, false, true, false, false, true })]
        [TestCase("test3", new byte[] { 5, 220 }, new[] { false, false, false, false, false, true, false, true, true, true, false, true, true, true, false, false })]
        public void ShouldReturnMostSignificantBitArrayFromMostSignificantByteArray(string label, byte[] msBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.MostSignificantByteArrayToMostSignificantBitArray(msBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(expectedLeastSignificantBits[i]));
            }
        }

        [Test]
        [TestCase("test1", new byte[] { 1 }, new[] { true, false, false, false, false, false, false, false })]
        [TestCase("test2", new byte[] { 9 }, new[] { true, false, false, true, false, false, false, false })]
        [TestCase("test3", new byte[] { 220, 5 }, new[] { false, false, true, true, true, false, true, true, true, false, true, false, false, false, false, false })]
        public void ShouldReturnLeastSignificantBitArrayFromLeastSignificantByteArray(string label, byte[] lsBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.LeastSignificantByteArrayToLeastSignificantBitArray(lsBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(expectedLeastSignificantBits[i]));
            }
        }

        [Test]
        [TestCase("test1", new byte[] { 1 }, new[] { false, false, false, false, false, false, false, true })]
        [TestCase("test2", new byte[] { 9 }, new[] { false, false, false, false, true, false, false, true })]
        [TestCase("test3", new byte[] { 220, 5 }, new[] { false, false, false, false, false, true, false, true, true, true, false, true, true, true, false, false })]
        public void ShouldReturnMostSignificantBitArrayFromLeastSignificantByteArray(string label, byte[] lsBytes, bool[] expectedLeastSignificantBits)
        {
            var result = MsbLsbConversionHelpers.LeastSignificantByteArrayToMostSignificantBitArray(lsBytes);

            for (int i = 0; i < result.Length; i++)
            {
                Assert.That(result[i], Is.EqualTo(expectedLeastSignificantBits[i]));
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

            Assert.That(bs.ToString(), Is.EqualTo(expectation));
        }
    }
}

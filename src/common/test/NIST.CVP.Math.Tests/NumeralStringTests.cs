using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;

namespace NIST.CVP.Math.Tests
{
    [TestFixture, UnitTest]
    public class NumeralStringTests
    {
        private NumeralString _subject;

        [Test]
        [TestCase(new int[] { 1, 2, 3 })]
        [TestCase(new int[] { 1, 2, 3, 4, 5 })]
        public void ShouldConstructWithShortsProperly(int[] values)
        {
            _subject = new NumeralString(values);

            Assert.Multiple(() =>
            {
                for (var i = 0; i < values.Length; i++)
                {
                    Assert.AreEqual(values[i], _subject.Numbers[i], $"{nameof(i)}: {i}");
                }
            });
        }

        [Test]
        [TestCase("1 2 3 4 5", new int[] { 1, 2, 3, 4, 5 })]
        public void ShouldConstructWithStringProperly(string value, int[] expectedValues)
        {
            _subject = new NumeralString(value);

            Assert.Multiple(() =>
            {
                for (var i = 0; i < expectedValues.Length; i++)
                {
                    Assert.AreEqual(expectedValues[i], _subject.Numbers[i], $"{nameof(i)}: {i}");
                }
            });
        }

        [Test]
        [TestCase("123 a 1231")]
        [TestCase("123 % 1231 564 877")]
        [TestCase("@ %")]
        [TestCase("!")]
        [TestCase("13 231 564 877F")]
        public void ShouldArgumentExceptionWithNonNumeralOrSpaces(string value)
        {
            Assert.Throws<ArgumentException>(() => _subject = new NumeralString(value));
        }

        [Test]
        [TestCase("65537000000000000")]
        [TestCase("1 2 3 10000000000000")]
        [TestCase("10000000000000000 1 2 3 ")]
        public void ShouldArgumentOutOfRangeExceptionWithNumbersGtShort(string value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _subject = new NumeralString(value));
        }

        [Test]
        [TestCase(new int[] { 1 }, "01")]
        [TestCase(new int[] { 1, 2 }, "0102")]
        [TestCase(new int[] { 1, 2, 15 }, "01020F")]
        [TestCase(new int[] { 1, 2, 255 }, "0102FF")]
        public void ShouldConvertToBitStringFromNumeralString(int[] values, string expectedHex)
        {
            _subject = new NumeralString(values);

            var result = NumeralString.ToBitString(_subject);

            Assert.AreEqual(expectedHex, result.ToHex());
        }

        [Test]
        [TestCase("01", new int[] { 1 })]
        [TestCase("0102", new int[] { 1, 2 })]
        [TestCase("01020F", new int[] { 1, 2, 15 })]
        [TestCase("0102FF", new int[] { 1, 2, 255 })]
        public void ShouldConvertToNumeralStringFromBitString(string hex, int[] expectedValues)
        {
            var bs = new BitString(hex);

            var result = NumeralString.ToNumeralString(bs);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.AreEqual(expectedValues[i], result.Numbers[i], $"{nameof(i)}: {i}");
            }
        }
    }
}
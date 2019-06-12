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
        [TestCase(new short[] { 1, 2, 3 })]
        [TestCase(new short[] { 1, 2, 3, 4, 5 })]
        public void ShouldConstructWithShortsProperly(short[] values)
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
        [TestCase("1 2 3 4 5", new short[] { 1, 2, 3, 4, 5 })]
        public void ShouldConstructWithStringProperly(string value, short[] expectedValues)
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
        [TestCase("65537")]
        [TestCase("1 2 3 100000000")]
        [TestCase("100000000 1 2 3 ")]
        public void ShouldArgumentOutOfRangeExceptionWithNumbersGtShort(string value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _subject = new NumeralString(value));
        }

        [Test]
        [TestCase(new short[] { 1 }, "0001")]
        [TestCase(new short[] { 1, 2 }, "00010002")]
        [TestCase(new short[] { 1, 2, 15 }, "00010002000F")]
        [TestCase(new short[] { 1, 2, 255 }, "0001000200FF")]
        public void ShouldConvertToBitStringFromNumeralString(short[] values, string expectedHex)
        {
            _subject = new NumeralString(values);

            var result = NumeralString.ToBitString(_subject);

            Assert.AreEqual(expectedHex, result.ToHex());
        }

        [Test]
        [TestCase("0001", new short[] { 1 })]
        [TestCase("00010002", new short[] { 1, 2 })]
        [TestCase("00010002000F", new short[] { 1, 2, 15 })]
        [TestCase("0001000200FF", new short[] { 1, 2, 255 })]
        public void ShouldConvertToNumeralStringFromBitString(string hex, short[] expectedValues)
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
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
        public void ShouldConstructWithIntssProperly(int[] values)
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
        [TestCase("1 2 3 4 5 65535", new int[] { 1, 2, 3, 4, 5 })]
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
        [TestCase("65537")]
        [TestCase("1 2 3 65536")]
        [TestCase("65537 1 2 3 ")]
        public void ShouldArgumentOutOfRangeExceptionWithNumbersGtTwoPow16(string value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => _subject = new NumeralString(value));
        }

        [Test]
        [TestCase(new int[] { 1 }, "0001")]
        [TestCase(new int[] { 1, 2 }, "00010002")]
        [TestCase(new int[] { 1, 2, 15 }, "00010002000F")]
        [TestCase(new int[] { 1, 2, 255 }, "0001000200FF")]
        public void ShouldConvertToBitStringFromNumeralString(int[] values, string expectedHex)
        {
            _subject = new NumeralString(values);

            var result = NumeralString.ToBitString(_subject);

            Assert.AreEqual(expectedHex, result.ToHex());
        }

        [Test]
        [TestCase("0001", new int[] { 1 })]
        [TestCase("00010002", new int[] { 1, 2 })]
        [TestCase("00010002000F", new int[] { 1, 2, 15 })]
        [TestCase("0001000200FF", new int[] { 1, 2, 255 })]
        public void ShouldConvertToNumeralStringFromBitString(string hex, int[] expectedValues)
        {
            var bs = new BitString(hex);

            var result = NumeralString.ToNumeralString(bs);

            for (var i = 0; i < expectedValues.Length; i++)
            {
                Assert.AreEqual(expectedValues[i], result.Numbers[i], $"{nameof(i)}: {i}");
            }
        }

        [Test]
        [TestCase("", false)]
        [TestCase("a", false)]
        [TestCase("aA", true)]
        [TestCase("0123456789", true)]
        [TestCase("01234567890", false)]
        public void ShouldDetermineIfAlphabetValid(string alphabet, bool expectedOutcome)
        {
            Assert.AreEqual(expectedOutcome, NumeralString.IsAlphabetValid(alphabet));
        }

        [Test]
        [TestCase("aB", new[] {0, 1, 0, 0}, true)]
        [TestCase("a", new[] {0, 0, 0}, false)]
        [TestCase("0123456789", new[] {0, 0, 0}, true)]
        [TestCase("0123456789", new[] {0, 0, 9, 10}, true)]
        [TestCase("0123456789", new[] {0, 0, 11}, false)]
        public void ShouldDetermineIfNumeralStringValidToAlphabet(string alphabet, int[] numbers, bool expectedOutcome)
        {
            var numeralString = new NumeralString(numbers);
            
            Assert.AreEqual(expectedOutcome, NumeralString.IsNumeralStringValidWithAlphabet(alphabet, numeralString));
        }

        [Test]
        [TestCase("012345", new[] {0, 1, 2, 2, 3, 3, 3}, "0122333")]
        [TestCase("54321", new[] {0, 1, 2, 2, 3, 3, 3}, "5433222")]
        [TestCase("abcdefghijklmnopqrstuvwxyz", new[] {7, 4, 11, 11, 14}, "hello")]
        public void ShouldToAlphabetString(string alphabet, int[] numbers, string expectation)
        {
            Assert.AreEqual(expectation, NumeralString.ToAlphabetString(alphabet, alphabet.Length, new NumeralString(numbers)));
        }
    }
}
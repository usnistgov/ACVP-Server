using System.Linq;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Domain
{
    [TestFixture, UnitTest]
    public class ValueDomainSegmentTests
    {
        private ValueDomainSegment _subject;

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldContainValueSetInConstructor(int value)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.IsWithinDomain(value);

            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase(5, 10)]
        [TestCase(10, 5)]
        public void ShouldSetValueIfParamMaxIsLessThanValue(int originalValue, int maxValue)
        {
            _subject = new ValueDomainSegment(originalValue);

            _subject.SetMaximumAllowedValue(maxValue);

            if (originalValue > maxValue)
            {
                Assert.That(_subject.GetSequentialValues(1).ToList()[0], Is.EqualTo(maxValue));
            }
            else
            {
                Assert.That(_subject.GetSequentialValues(1).ToList()[0], Is.EqualTo(originalValue));
            }
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldBeSameMinMax(int value)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.RangeMinMax;

            Assert.That(result.Minimum, Is.EqualTo(value), "minimum");
            Assert.That(result.Maximum, Is.EqualTo(value), "maximum");
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldReturnValueFromConstructorUsingGetValues(int value)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.GetSequentialValues(1).ToList();

            Assert.That(result[0], Is.EqualTo(value));
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldReturnSingleValueFromSegmentWhenAskingForMultiple(int value)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.GetSequentialValues(5).ToList();

            Assert.That(result[0], Is.EqualTo(value), nameof(value));
            Assert.That(result.Count, Is.EqualTo(1), nameof(result.Count));
        }

        [Test]
        [TestCase(1, true)]
        [TestCase(6, true)]
        //[TestCase(int.MaxValue, true)] Doesn't pass because there is a max limit to ValueDomainSegment
        [TestCase(0, false)]
        [TestCase(-6, false)]
        [TestCase(int.MinValue, false)]
        public void ShouldReturnSingleValueThatMatchesCondition(int value, bool resultShouldHaveContent)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.GetSequentialValues(v => v > 0, 1).ToList();

            Assert.That(result.Count != 0, Is.EqualTo(resultShouldHaveContent));

            if (resultShouldHaveContent)
            {
                Assert.That(result[0], Is.EqualTo(value));
            }
        }

        [Test]
        [TestCase(5, 5, 10, true)]
        [TestCase(7, 5, 10, true)]
        [TestCase(10, 5, 10, true)]
        [TestCase(4, 5, 10, false)]
        [TestCase(11, 5, 10, false)]
        public void ShouldReturnValueWhenMinSpecifiedIsValue(int value, int minimum, int maximum, bool expectItemReturned)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.GetSequentialValues(minimum, maximum, 1).ToList();

            Assert.That(result.Count == 1, Is.EqualTo(expectItemReturned));
        }
    }
}

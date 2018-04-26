using System.Linq;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Domain
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

            Assert.IsTrue(result);
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
                Assert.AreEqual(maxValue, _subject.GetValues(1).ToList()[0]);
            }
            else
            {
                Assert.AreEqual(originalValue, _subject.GetValues(1).ToList()[0]);
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

            Assert.AreEqual(value, result.Minimum, "minimum");
            Assert.AreEqual(value, result.Maximum, "maximum");
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldReturnValueFromConstructorUsingGetValues(int value)
        {
            _subject = new ValueDomainSegment(value);

            var result = _subject.GetValues(1).ToList();

            Assert.AreEqual(value, result[0]);
        }

        [Test]
        [TestCase(0)]
        [TestCase(5)]
        [TestCase(10)]
        public void ShouldReturnSingleValueFromSegmentWhenAskingForMultiple(int value)
        {
            _subject = new ValueDomainSegment(value);
            
            var result = _subject.GetValues(5).ToList();

            Assert.AreEqual(value, result[0], nameof(value));
            Assert.AreEqual(1, result.Count, nameof(result.Count));
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

            var result = _subject.GetValues(v => v > 0, 1).ToList();

            Assert.AreEqual(resultShouldHaveContent, result.Count != 0);

            if (resultShouldHaveContent)
            {
                Assert.AreEqual(value, result[0]);
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

            var result = _subject.GetValues(minimum, maximum, 1).ToList();
            
            Assert.AreEqual(expectItemReturned, result.Count == 1);
        }
    }
}

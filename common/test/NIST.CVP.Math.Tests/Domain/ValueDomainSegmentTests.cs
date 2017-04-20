using System.Linq;
using NIST.CVP.Math.Domain;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Domain
{
    [TestFixture]
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
    }
}

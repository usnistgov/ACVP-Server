using System;
using System.Linq;
using Moq;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Domain
{
    [TestFixture, UnitTest]
    public class MathDomainTests
    {
        [SetUp]
        public void Setup()
        {
            _mockDomainSegment = new Mock<IDomainSegment>();
            _subject = new MathDomain();
            _subject.AddSegment(_mockDomainSegment.Object);
        }

        private MathDomain _subject;
        private Mock<IDomainSegment> _mockDomainSegment;

        [Test]
        public void AddSegmentShouldAddSegmentToCollection()
        {
            var segmentCount = _subject.DomainSegments.Count();

            _subject.AddSegment(_mockDomainSegment.Object);

            Assert.AreEqual(segmentCount + 1, _subject.DomainSegments.Count());
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldSetSegmentValueOptionsForEachSegment(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.SetRangeOptions(It.IsAny<RangeDomainSegmentOptions>());

            _mockDomainSegment.VerifySet(v => v.SegmentValueOptions = It.IsAny<RangeDomainSegmentOptions>(), Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldSetMaxAllowedValueForEachSegment(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.SetMaximumAllowedValue(100);

            _mockDomainSegment.Verify(v => v.SetMaximumAllowedValue(It.IsAny<int>()), Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldCallGetMinMaxFromSegmentsForEachSegment(int numberOfSegments)
        {
            int min = 0;
            int max = 100;

            _mockDomainSegment
                .SetupGet(s => s.RangeMinMax)
                .Returns(new RangeMinMax
                {
                    Minimum = min,
                    Maximum = max
                });

            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.GetDomainMinMax();

            _mockDomainSegment.VerifyGet(v => v.RangeMinMax, Times.Exactly(numberOfSegments));
        }

        [Test]
        public void ShouldGetCorrectMinMaxFromSegments()
        {
            int min = 0;
            int max = 100;

            _mockDomainSegment
                .SetupGet(s => s.RangeMinMax)
                .Returns(new RangeMinMax
                {
                    Minimum = min,
                    Maximum = max
                });

            var result = _subject.GetDomainMinMax();

            Assert.AreEqual(min, result.Minimum, nameof(min));
            Assert.AreEqual(max, result.Maximum, nameof(max));
        }

        [Test]
        public void ShouldNotSayDomainContainsOtherElements()
        {
            int min = 100;
            int max = 100;

            _mockDomainSegment
                .SetupGet(s => s.RangeMinMax)
                .Returns(new RangeMinMax
                {
                    Minimum = min,
                    Maximum = max
                });

            var result = _subject.ContainsValueOtherThan(100);

            Assert.IsFalse(result);
        }

        [Test]
        public void ShouldSayDomainContainsOtherElements()
        {
            int min = 1;
            int max = 100;

            _mockDomainSegment
                .SetupGet(s => s.RangeMinMax)
                .Returns(new RangeMinMax
                {
                    Minimum = min,
                    Maximum = max
                });

            var result = _subject.ContainsValueOtherThan(100);

            Assert.IsTrue(result);
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldGetRangeMinMaxForEachSegment(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.GetMinMaxRanges();

            _mockDomainSegment.VerifyGet(v => v.RangeMinMax, Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldCallIsWithinDomainForEachSegment(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.IsWithinDomain(It.IsAny<int>());

            _mockDomainSegment.Verify(v => v.IsWithinDomain(It.IsAny<int>()), Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldCallGetValuesForEachSegment(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.GetValues(It.IsAny<int>());

            _mockDomainSegment.Verify(v => v.GetValues(It.IsAny<int>()), Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(1)]
        [TestCase(5)]
        public void ShouldCallGetValuesForEachSegmentWithRange(int numberOfSegments)
        {
            while (_subject.DomainSegments.Count() < numberOfSegments)
            {
                _subject.AddSegment(_mockDomainSegment.Object);
            }

            _subject.GetValues(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

            _mockDomainSegment.Verify(v => v.GetValues(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(numberOfSegments));
        }

        [Test]
        [TestCase(2, 1)]
        public void ShouldArgumentExceptionWhenMinGtThanMaxGetValues(int min, int max)
        {
            Assert.Throws(typeof(ArgumentException), () => _subject.GetValues(min, max, 0));
        }

        [Test]
        public void ShouldOnlyReturnValidValuesFromGetValues()
        {
            var domain = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 64, 128, 64));
            domain.SetRangeOptions(RangeDomainSegmentOptions.Random);

            var values = domain.GetValues(128).ToList();

            Assert.AreEqual(2, values.Count());
            Assert.IsTrue(values.Contains(64));
            Assert.IsTrue(values.Contains(128));
        }
    }
}

using System.Linq;
using Moq;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Math.Tests.Domain
{
    [TestFixture, UnitTest]
    public class MathDomainTests
    {
        private MathDomain _subject;
        private Mock<IDomainSegment> _mockDomainSegment;

        [SetUp]
        public void Setup()
        {
            _mockDomainSegment = new Mock<IDomainSegment>();
            _subject = new MathDomain();
            _subject.AddSegment(_mockDomainSegment.Object);
        }

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
                .Returns(new RangeMinMax()
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
                .Returns(new RangeMinMax()
                {
                    Minimum = min,
                    Maximum = max
                });

            var result = _subject.GetDomainMinMax();

            Assert.AreEqual(min, result.Minimum, nameof(min));
            Assert.AreEqual(max, result.Maximum, nameof(max));
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
    }
}

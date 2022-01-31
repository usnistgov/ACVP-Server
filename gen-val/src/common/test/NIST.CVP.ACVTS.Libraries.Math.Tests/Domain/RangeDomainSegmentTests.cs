using System;
using System.Linq;
using Moq;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Math.Tests.Domain
{
    [TestFixture, UnitTest]
    public class RangeDomainSegmentTests
    {
        [SetUp]
        public void Setup()
        {
            _mockRandom = new Mock<IRandom800_90>();
        }

        private Mock<IRandom800_90> _mockRandom;
        private RangeDomainSegment _subject;

        [Test]
        [TestCase(2, 1)]
        [TestCase(2, 2)]
        [TestCase(100, 10)]
        public void ShouldThrowExceptionWhenMinGreaterThanMax(int min, int max)
        {
            Assert.Throws(typeof(ArgumentException), () => _subject = new RangeDomainSegment(_mockRandom.Object, min, max));
        }

        [Test]
        [TestCase(0)]
        [TestCase(-1)]
        public void ShouldThrowExceptionWhenIncrementLessThanOne(int increment)
        {
            Assert.Throws(typeof(ArgumentException), () => _subject = new RangeDomainSegment(_mockRandom.Object, 1, 100, increment));
        }

        [Test]
        [TestCase(1, 3, 4)]
        [TestCase(5, 10, 10)]
        [TestCase(1, 2, 2)]
        public void ShouldThrowExceptionWhenMaxMinusMinLessThanIncrement(int min, int max, int increment)
        {
            Assert.Throws(typeof(ArgumentException), () => _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment));
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(1, 10)]
        [TestCase(50, 100)]
        public void ShouldReturnProperMinMaxAsSetInConstructor(int min, int max)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max);

            var result = _subject.RangeMinMax;

            Assert.AreEqual(min, result.Minimum, nameof(min));
            Assert.AreEqual(max, result.Maximum, nameof(max));
        }

        [Test]
        [TestCase(RangeDomainSegmentOptions.Random)]
        [TestCase(RangeDomainSegmentOptions.Sequential)]
        public void ShouldGetSetSegmentValueOptionsWhenValuesHaventGenerated(RangeDomainSegmentOptions option)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10);
            _subject.SegmentValueOptions = option;

            Assert.AreEqual(option, _subject.SegmentValueOptions);
        }

        [Test]
        public void ShouldBeSequentialByDefault()
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10);
            Assert.AreEqual(RangeDomainSegmentOptions.Sequential, _subject.SegmentValueOptions);
        }

        [Test]
        public void ShouldThrowExceptionWhenChangingOptionsAfterGetValues()
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10);
            _subject.SegmentValueOptions = RangeDomainSegmentOptions.Sequential;

            _subject.GetValues(_subject.MaxNumberOfValuesInSegment);

            Assert.Throws(typeof(NotSupportedException),
                () => _subject.SegmentValueOptions = RangeDomainSegmentOptions.Sequential);
        }

        [Test]
        [TestCase(5, 10)]
        [TestCase(10, 5)]
        public void ShouldSetInstanceMaxIfParamMaxIsGreaterThanValue(int originalValue, int maxValue)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, originalValue);
            _subject.SetMaximumAllowedValue(maxValue);

            if (originalValue > maxValue)
            {
                Assert.AreEqual(maxValue, _subject.RangeMinMax.Maximum);
            }
            else
            {
                Assert.AreEqual(originalValue, _subject.RangeMinMax.Maximum);
            }
        }

        [Test]
        [TestCase(50, 10)]
        [TestCase(10, 50)]
        public void ShouldSetInstanceMinIfParamMaxIsGreaterThanValue(int originalValue, int maxValue)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, originalValue, 100);
            _subject.SetMaximumAllowedValue(maxValue);

            if (originalValue > maxValue)
            {
                Assert.AreEqual(0, _subject.RangeMinMax.Minimum);
            }
            else
            {
                Assert.AreEqual(originalValue, _subject.RangeMinMax.Minimum);
            }
        }

        [Test]
        public void ShouldThrowExceptionWhenOptionsChangedAfterGetValuesInvoked()
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10);
            _subject.GetValues(_subject.MaxNumberOfValuesInSegment);

            Assert.Throws(typeof(NotSupportedException), () => _subject.SetMaximumAllowedValue(100));
        }

        [Test]
        [TestCase(1, 10, 1, 2, true)]
        [TestCase(1, 10, 1, 3, true)]
        [TestCase(2, 10, 2, 4, true)]
        [TestCase(2, 10, 2, 3, false)]
        [TestCase(1, 11, 2, 10, false)]
        public void ShouldReturnCorrectResponseIfNumberIncludedInRange(int min, int max, int increment,
            int numberToCheck, bool expectation)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment);

            var result = _subject.IsWithinDomain(numberToCheck);

            Assert.AreEqual(expectation, result);
        }

        [Test]
        [TestCase(0, 10, 1, 11)]
        [TestCase(0, 10, 2, 6)]
        [TestCase(1, 10, 3, 4)]
        [TestCase(1, int.MaxValue, 1, RangeDomainSegment._MAXIMUM_ALLOWED_RETURNS)]
        public void ShouldReturnCorrectMaximumAvailableInSegment(int min, int max, int increment, int expectation)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment);

            var result = _subject.MaxNumberOfValuesInSegment;

            Assert.AreEqual(expectation, result);
        }

        [Test]
        [TestCase(1, 10, 1)]
        [TestCase(1, 11, 2)]
        public void ShouldReturnNumbersOnlyUpToMaximumAvailable(int min, int max, int increment)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Sequential
            };

            int maxQuantityAvailable = _subject.MaxNumberOfValuesInSegment;

            var result = _subject.GetValues(maxQuantityAvailable).ToList();

            Assert.AreEqual(_subject.MaxNumberOfValuesInSegment, result.Count);
        }

        private static object[] _sequenceTestData = {
            new object[] {1, 5, 1, new[] {1, 2, 3, 4, 5}},
            new object[] {1, 5, 2, new[] {1, 3, 5}},
            new object[] {2, 6, 2, new[] {2, 4, 6}},
            new object[] {0, 10, 2, new[] {0, 2, 4, 6, 8, 10}}
        };

        private static object[] _randomTestData = {
            new object[] { "1-5 step 1, start at 3, return up to 5 values", 1, 5, 1, 3, 5, new[] { 3, 4, 2, 5, 1 } },
            new object[] { "1-9 step 1, start at 5, return up to 5 values", 1, 9, 1, 5, 5, new[] { 5, 6, 4, 7, 3 } },
            new object[] { "0-12 step 4, start at 5, return up to 1 values", 0, 12, 4, 5, 1, new[] { 4 } },
            new object[] { "0-12 step 4, start at 5, return up to 2 values", 0, 12, 4, 5, 2, new[] { 4, 8 } },
            new object[] { "0-10 step 2, start at 5, return up to 4 values", 0, 10, 2, 5, 4, new[] { 6, 4, 8, 2 } },
            new object[] { "0-10 step 2, start at 5, return up to 10 values", 0, 10, 2, 5, 10, new[] { 6, 4, 8, 2, 10, 0 } },
            new object[] { "0-10 step 2, start at 4, return up to 10 values", 0, 10, 2, 4, 10, new[] { 4, 6, 2, 8, 0, 10 } }
        };

        [Test]
        [TestCaseSource(nameof(_sequenceTestData))]
        public void ShouldReturnNumbersInSequence(int min, int max, int increment, int[] expectation)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Sequential
            };

            var maxQuantityAvailable = _subject.MaxNumberOfValuesInSegment;

            var result = _subject.GetValues(maxQuantityAvailable).ToList();

            for (int i = 0; i < expectation.Length; i++)
            {
                Assert.AreEqual(expectation[i], result[i]);
            }
        }

        /// <summary>
        /// Using the first case as an example ([TestCase(1, 5, 1, 3, new int[] { 2, 3, 1 } )])
        /// If we cause our random generator to always return 3, given a range of 1-5, increment of one.
        /// The first value returned should be 3, 
        /// the second should be 4 (initial value + 1), 
        /// the third value returned should be 2 (initial value - 1)
        /// the Fourth is 5 (no values +/- 1 are valid, as they've already been returned in the set, move on to +/- 2
        /// the final is 1 (initial value - 2)
        /// </summary>
        /// <param name="min">Minimum of range</param>
        /// <param name="max">Maximum of range</param>
        /// <param name="increment">The increment to use</param>
        /// <param name="seedRandom">The value random should return with each call</param>
        /// <param name="quantityToReturn">Number of values to return</param>
        /// <param name="expectation">The expected random return</param>
        [Test]
        [TestCaseSource(nameof(_randomTestData))]
        public void ShouldReturnValidNumbersWithinRangeRandomly(string testLabel, int min, int max, int increment, int seedRandom, int quantityToReturn, int[] expectation)
        {
            // Random should always return the "seedRandom" in order to test the +/- logic
            _mockRandom
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(seedRandom);

            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Random
            };

            var result = _subject.GetValues(quantityToReturn);

            Assert.AreEqual(expectation.ToList(), result.ToList());
        }

        [Test]
        [TestCaseSource(nameof(_sequenceTestData))]
        public void ShouldReturnNumbersInSequenceSubsetRangeSanityCheck(int min, int max, int increment, int[] expectation)
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Sequential
            };

            var maxQuantityAvailable = _subject.MaxNumberOfValuesInSegment;

            var result = _subject.GetValues(min, max, maxQuantityAvailable).ToList();

            for (int i = 0; i < expectation.Length; i++)
            {
                Assert.AreEqual(expectation[i], result[i]);
            }
        }

        /// <summary>
        /// Using the first case as an example ([TestCase(1, 5, 1, 3, new int[] { 2, 3, 1 } )])
        /// If we cause our random generator to always return 3, given a range of 1-5, increment of one.
        /// The first value returned should be 3, 
        /// the second should be 4 (initial value + 1), 
        /// the third value returned should be 2 (initial value - 1)
        /// the Fourth is 5 (no values +/- 1 are valid, as they've already been returned in the set, move on to +/- 2
        /// the final is 1 (initial value - 2)
        /// </summary>
        /// <param name="min">Minimum of range</param>
        /// <param name="max">Maximum of range</param>
        /// <param name="increment">The increment to use</param>
        /// <param name="seedRandom">The value random should return with each call</param>
        /// <param name="quantityToReturn">Number of values to return</param>
        /// <param name="expectation">The expected random return</param>
        [Test]
        [TestCaseSource(nameof(_randomTestData))]
        public void ShouldReturnValidNumbersWithinRangeRandomlySanityCheck(string testLabel, int min, int max, int increment, int seedRandom, int quantityToReturn, int[] expectation)
        {
            // Random should always return the "seedRandom" in order to test the +/- logic
            _mockRandom
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(seedRandom);

            _subject = new RangeDomainSegment(_mockRandom.Object, min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Random
            };

            var result = _subject.GetValues(min, max, quantityToReturn);

            Assert.AreEqual(expectation.ToList(), result.ToList());
        }

        [Test]
        public void ShouldReturnSequentialValuesOnlyWithinSubsets()
        {
            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Sequential
            };

            int minSubset = 0;
            int maxSubset = 4;
            int[] expectedValues = { 0, 1, 2, 3, 4 };

            var maxQuantityAvailable = _subject.MaxNumberOfValuesInSegment;

            var result = _subject.GetValues(minSubset, maxSubset, maxQuantityAvailable).ToList();

            Assert.IsTrue(expectedValues.OrderBy(t => t).SequenceEqual(result.OrderBy(t => t)));
        }

        [Test]
        public void ShouldReturnRandomValuesOnlyWithinSubsets()
        {
            // Random should always return the "seedRandom" in order to test the +/- logic
            _mockRandom
                .Setup(s => s.GetRandomInt(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(0);

            _subject = new RangeDomainSegment(_mockRandom.Object, 0, 10)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Random
            };

            int minSubset = 0;
            int maxSubset = 4;
            int[] expectedValues = { 0, 1, 2, 3, 4 };

            var maxQuantityAvailable = _subject.MaxNumberOfValuesInSegment;

            var result = _subject.GetValues(minSubset, maxSubset, maxQuantityAvailable);

            Assert.AreEqual(expectedValues.ToList(), result.ToList());
        }

        [Test]
        public void ShouldProduceValuesWithConditionBoundToDomain()
        {
            _subject = new RangeDomainSegment(new Random800_90(), 8, 128, 8);

            var values = _subject.GetValues(v => v > 64, 10);

            Assert.IsTrue(values.All(a => a % 8 == 0));
        }

        [Test]
        // 1048576 is the largest value supported by a RangeDomainSegment
        [TestCase(0, 1048576, 1)]
        [TestCase(0, 10, 1)]
        [TestCase(-1024, 1024, 1)]
        [TestCase(0, 65536 * 2, 2)]
        [TestCase(0, 1048575, 3)]
        [TestCase(128, 512, 128)]
        public void ShouldReturnValuesThatMatchAGivenCondition(int min, int max, int increment)
        {
            _subject = new RangeDomainSegment(new Random800_90(), min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Random
            };

            var result = _subject.GetValues(v => v % 8 == 0, 10);

            Assert.LessOrEqual(result.Count(), 10);

            foreach (var value in result)
            {
                Assert.IsTrue(value % 8 == 0);
            }
        }

        [Test]
        [TestCase(128, 128 * 200, 1)]
        [TestCase(1, 128 * 5, 1)]
        public void ShouldAlwaysPullMaximumNumberOfValuesWithACondition(int min, int max, int increment)
        {
            _subject = new RangeDomainSegment(new Random800_90(), min, max, increment)
            {
                SegmentValueOptions = RangeDomainSegmentOptions.Random
            };

            for (var i = 0; i < 100; i++)
            {
                var result = _subject.GetValues(v => v % 128 == 0, 5);
                Assert.AreEqual(result.Count(), 5, "pulled not enough mod 128");
            }

            for (var i = 0; i < 100; i++)
            {
                var result = _subject.GetValues(v => v % 128 != 0, 5);
                Assert.AreEqual(result.Count(), 5, "pulled not enough not mod 128");
            }
        }
    }
}

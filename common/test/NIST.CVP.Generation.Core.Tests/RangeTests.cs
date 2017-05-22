using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.Core.Tests
{
    [TestFixture, UnitTest]
    public class RangeTests
    {
        private Range _subject = new Range();

        [Test]
        [TestCase(1, 2, 1)]
        [TestCase(0, 2, 0)]
        [TestCase(500, 1000, 500)]
        [TestCase(1, 1, 1)]
        public void ShouldReturnProperMin(int min, int max, int expectation)
        {
            _subject = new Range()
            {
                Min = min,
                Max = max
            };
            
            Assert.AreEqual(expectation, _subject.Min);
        }

        [Test]
        [TestCase(1, 2, 2)]
        [TestCase(0, 2, 2)]
        [TestCase(500, 1000, 1000)]
        [TestCase(1, 1, 1)]
        public void ShouldReturnProperMax(int min, int max, int expectation)
        {
            _subject = new Range()
            {
                Min = min,
                Max = max
            };

            Assert.AreEqual(expectation, _subject.Max);
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(0, 2)]
        [TestCase(500, 1000)]
        [TestCase(1, 1)]
        public void ShouldReturnProperMinMaxEnumerable(int min, int max)
        {
            _subject = new Range()
            {
                Min = min,
                Max = max
            };

            var result = _subject.GetMinMaxAsEnumerable().ToList();

            Assert.AreEqual(min, result[0]);
            Assert.AreEqual(max, result[1]);
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(0, 2)]
        [TestCase(500, 1000)]
        [TestCase(1, 1)]
        public void ShouldReturnProperValues(int min, int max)
        {
            _subject = new Range()
            {
                Min = min,
                Max = max
            };

            var result = _subject.GetValues().ToList();

            Assert.IsTrue(result.Count() <= 100, "by default, a maximum of 100 values should be returned");

            int numberToReturn = max - min < 100 ? max - min : 100;

            for (int i = min; i <= numberToReturn; i++)
            {
                Assert.AreEqual(i, result[i - min]);
            }
        }
    }
}

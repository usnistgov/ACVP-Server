using NIST.CVP.Generation.KMAC.v1_0;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorAlgorithmFunctionalTests
    {
        private TestGroupGeneratorAlgorithmFunctional _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorAlgorithmFunctional();
        }

        private static object[] testData = new[]
        {
            // 0
            new object[]
            {
                "test1 - 0",
                new int[] {},
                0
            },
            // 1 
            new object[]
            {
                "test2 - 1",
                new int[] { 128 },
                1
            },
            // 2 
            new object[]
            {
                "test3 - 2",
                new int[] { 128, 256 },
                2
            }
        };

        [Test]
        [TestCaseSource(nameof(testData))]
        public async Task ShouldReturnOneITestGroupForEveryDigestSize(
            string label,
            int[] digestSizes,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "KMAC",
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                XOF = new[] { false },
                DigestSizes = digestSizes,
                IsSample = true
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount, result.Count());
        }

        [Test]
        [TestCaseSource(nameof(testData))]
        public async Task ShouldReturnTwoITestGroupsForEveryDigestSizeXOF(
            string label,
            int[] digestSizes,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "KMAC",
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                XOF = new[] { true, false },
                DigestSizes = digestSizes,
                IsSample = true
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount * 2, result.Count());
        }

        [Test]
        [TestCaseSource(nameof(testData))]
        public async Task ShouldReturnOneITestGroupForEveryDigestSizeNonXOF(
            string label,
            int[] digestSizes,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "KMAC",
                MsgLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                KeyLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                MacLen = new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 1024, 8)),
                XOF = new[] { true },
                DigestSizes = digestSizes,
                IsSample = true
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.AreEqual(expectedResultCount, result.Count());
        }
    }
}

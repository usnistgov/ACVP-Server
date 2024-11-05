using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.HMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.HMAC
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private TestGroupGenerator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGenerator();
        }

        private static object[] testData = new[]
        {
            // 2
            new object[]
            {
                "test1 - 1",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)), // note this is an invalid MAC length, but validated as such in the parameters validator
                2
            },
            // 2 (1*2)
            new object[]
            {
                "test2 - 2",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(80)),
                2
            },
            // 4 (1*4)
            new object[]
            {
                "test3 - 4",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                // 4 valid segments in this range for sha1
                new MathDomain()
                    .AddSegment(new ValueDomainSegment(80))
                    .AddSegment(new ValueDomainSegment(96))
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(160)),
                4
            },
            // 4 (1*4)
            new object[]
            {
                "test4 - 4",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                // 4 valid segments in this range for sha1
                new MathDomain()
                    .AddSegment(new ValueDomainSegment(80))
                    .AddSegment(new ValueDomainSegment(96))
                    .AddSegment(new ValueDomainSegment(128))
                    .AddSegment(new ValueDomainSegment(160))
                    .AddSegment(new ValueDomainSegment(88)), // invalid for sha1, don't include
                4
            },
            // 4 (1*4)
            new object[]
            {
                "test5 - 4",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                // 4 valid segments in this range for sha1
                new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)), // 4
                4
            },
            // 13
            new object[]
            {
                "test5 - 13",
                // pull 5 random values (5 < block size, 1 = block size, 5 > block size
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)),
                // 4 valid segments in this range for sha1
                new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                13
            },
        };

        [Test]
        [TestCaseSource(nameof(testData))]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            MathDomain keyLen,
            MathDomain macLen,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "HMAC-SHA-1",
                KeyLen = keyLen,
                MacLen = macLen
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count(), Is.EqualTo(expectedResultCount));
        }
    }
}

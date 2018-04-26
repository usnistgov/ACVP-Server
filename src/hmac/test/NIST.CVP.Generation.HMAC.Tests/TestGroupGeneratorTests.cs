using System;
using System.Linq;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
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
            // 0
            new object[]
            {
                "test1 - 1",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)), // note this is an invalid MAC length, but validated as such in the parameters validator
                1
            },
            // 1 (1*1)
            new object[]
            {
                "test2 - 1",
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(80)),
                1
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
            // 4 (5*4)
            new object[]
            {
                "test5 - 20",
                // pull 5 random values (2 < block size, 1 = block size, 2 > block size
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024)),
                // 4 valid segments in this range for sha1
                new MathDomain()
                    .AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)), // 4
                20
            },
        };
        
        [Test]
        [TestCaseSource(nameof(testData))]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
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

            var result = _subject.BuildTestGroups(p);

            Assert.AreEqual(expectedResultCount, result.Count());
        }
    }
}

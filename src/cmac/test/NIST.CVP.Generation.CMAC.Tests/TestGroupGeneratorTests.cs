using System.Linq;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.CMAC.Tests
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
            new object[]
            {
                // 0
                "test1 - 0",
                string.Empty,
                "AES",
                256,
                0,
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                1
            },
            new object[]
            {
                // 1 (1*1*1*1)
                "test2 - 3",
                "gen",
                "AES",
                128,
                0,
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                new MathDomain().AddSegment(new ValueDomainSegment(8)),
                1
            },
            new object[]
            {
                "test3 - 1620",
                "var",
                "TDES",
                0,
                1,
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                9 // 3 groups per domain segment, cartesian joined.
            }
        };
        
        [Test]
        [TestCaseSource(nameof(testData))]
        public void ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            string direction,
            string mode,
            int keyLen,
            int keyingOption,
            MathDomain msgLen,
            MathDomain macLen,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "CMAC",
                Mode = mode,
                Capabilities = new[]
                {
                    new Capability()
                    {
                        Direction = direction,
                        KeyLen = keyLen,
                        KeyingOption = keyingOption,
                        MsgLen = msgLen,
                        MacLen = macLen
                    }
                }
            };

            var result = _subject.BuildTestGroups(p);

            Assert.AreEqual(expectedResultCount, result.Count());
        }
    }
}

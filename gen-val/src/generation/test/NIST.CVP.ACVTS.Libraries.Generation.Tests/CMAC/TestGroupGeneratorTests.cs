using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.CMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.CMAC
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

        private static object[] testData =
        {
            new object[]
            {
                // 0
                "test1 - 0",
                string.Empty,
                "CMAC-AES",
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
                "CMAC-AES",
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
                "CMAC-TDES",
                0,
                1,
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)),
                9 // 3 groups per domain segment, cartesian joined.
            }
        };

        [Test]
        [TestCaseSource(nameof(testData))]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamters(
            string label,
            string direction,
            string algorithm,
            int keyLen,
            int keyingOption,
            MathDomain msgLen,
            MathDomain macLen,
            int expectedResultCount
        )
        {
            var p = new Parameters
            {
                Algorithm = algorithm,
                Capabilities = new[]
                {
                    new Capability
                    {
                        Direction = new [] {direction},
                        KeyLen = new [] {keyLen},
                        KeyingOption = new [] {keyingOption},
                        MsgLen = msgLen,
                        MacLen = macLen
                    }
                }
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count(), Is.EqualTo(expectedResultCount));
        }
    }
}

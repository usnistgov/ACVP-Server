using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CFB.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CFB
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorKnownAnswerTests
    {
        private TestGroupGeneratorKnownAnswer _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestGroupGeneratorKnownAnswer();
        }

        [Test]
        // 0
        [TestCase(
            "test1 - 0",
            new string[] { },
            new int[] { },
            0
        )]
        // 0
        [TestCase(
            "test2 - 0",
            new string[] { },
            new int[] { 2 },
            0
        )]
        // 5 (1*5) // 1 directions, 5 kat types
        [TestCase(
            "test3 - 5",
            new string[] { "decrypt" },
            new int[] { 1 },
            5
        )]
        // 5 (1*5) // 1 directions, 5 kat types
        [TestCase(
            "test4 - 5",
            new string[] { "encrypt" },
            new int[] { 1 },
            5
        )]
        // 10 (2*5) // 2 directions, 5 kat types
        [TestCase(
            "test5 - 10",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1 },
            10
        )]
        // 10 (2*5) - 2 directions, 5 kat types, keying option parameter makes no difference to kats
        [TestCase(
            "test6 - 10",
            new string[] { "encrypt", "decrypt" },
            new int[] { 1, 2 },
            10
        )]
        public async Task ShouldReturnOneITestGroupForEveryMultiplicativeIterationOfParamtersWithNoKatOrMctImpl(
            string label,
            string[] mode,
            int[] keyOption,
            int expectedResultCount
        )
        {
            Parameters p = new Parameters()
            {
                Algorithm = "ACVP-TDES-CFB1",
                Revision = "1.0",
                Direction = mode,
                KeyingOption = keyOption,
            };

            var result = await _subject.BuildTestGroupsAsync(p);

            Assert.That(result.Count, Is.EqualTo(expectedResultCount));
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorCounterTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 0 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new int[] { }) // 0
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithOverflowCounter(false)
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithOverflowCounter(false)
                    .Build()
            },
            new object[]
            {
                3, // 3 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithOverflowCounter(false) // 1
                    .Build()
            },
            new object[]
            {
                6, // 3 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithOverflowCounter(true)
                    .Build()
            },
            new object[]
            {
                0,
                new ParameterBuilder()
                    .WithPerformCounterTests(false)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfKeyingOptionAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorCounter();

            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorSingleBlockMessageTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 0 * 1
                new ParameterBuilder()
                    .WithKeyingOption(new int[] { }) // 0
                    .WithDirection(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 * 0
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                2, // 2 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            },
            new object[]
            {
                3, // 3 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorSingleBlockMessage();

            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}

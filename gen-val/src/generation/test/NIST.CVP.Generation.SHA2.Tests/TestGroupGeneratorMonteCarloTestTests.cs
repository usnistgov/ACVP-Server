using NIST.CVP.Generation.SHA2.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMonteCarloTestTests
    {
        private static object[] parameters = 
        {
            new object[]
            {
                0, // 1 * 0
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new string[] { }) // 0
                    .WithAlgorithm("SHA2")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new string[] { "224", "256" }) // 2
                    .WithAlgorithm("SHA2")  // 1
                    .Build()
            },
            new object[]
            {
                3, // 1 * 3
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new string[] { "512", "512/224", "512/256" }) // 3
                    .WithAlgorithm("SHA2")  // 1
                    .Build()
            },
            new object[]
            {
                6, // 1 * 6
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new string[] { "224", "256", "384", "512", "512/224", "512/256" }) // 6
                    .WithAlgorithm("SHA2")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorMonteCarloTest();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

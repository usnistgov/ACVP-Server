using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorAlgorithmFunctionalTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 1 * 0
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("cSHAKE")  // 1
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("cSHAKE")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("cSHAKE")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorAlgorithmFunctional();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}

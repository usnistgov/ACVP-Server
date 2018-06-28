using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMonteCarloTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 1 * 0 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            },
            new object[]
            {
                4, // 1 * 2 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate2TestGroupsForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorMonteCarlo();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());

            var totalXOFModes = 0;
            var totalNotXOFModes = 0;
            foreach (var result in results)
            {
                if (result.XOF)
                {
                    totalXOFModes++;
                }
                else
                {
                    totalNotXOFModes++;
                }
            }
            Assert.AreEqual(totalXOFModes, totalNotXOFModes);
        }
    }
}

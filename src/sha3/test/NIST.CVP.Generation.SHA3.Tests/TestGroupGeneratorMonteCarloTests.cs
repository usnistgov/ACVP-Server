using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorMonteCarloTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 1 * 0
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("SHA3")  // 1
                    .Build()
            },
            new object[]
            {
                1, // 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 256 }) // 2
                    .WithAlgorithm("SHA3")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("SHAKE")  // 1
                    .Build()
            },
            new object[]
            {
                4, // 1 * 4
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 224, 256, 384, 512 }) // 4
                    .WithAlgorithm("SHA3")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorMonteCarlo();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
        
    }
}

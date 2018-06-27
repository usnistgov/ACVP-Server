using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.ParallelHash.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorVariableOutputTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 1 * 0 
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2 
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorVariableOutput();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

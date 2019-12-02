using System.Linq;
using NIST.CVP.Generation.ParallelHash.v1_0;
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
                    .WithXOF(new []{true})
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 * 1
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new []{true})
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2 * 1
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new []{true})
                    .Build()
            },
            new object[]
            {
                1, // 1 * 1 * 1
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new []{true})
                    .Build()
            },
            new object[]
            {
                2, // 1 * 2 * 1
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new []{true})
                    .Build()
            },
            new object[]
            {
                2, // 1 * 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new [] {true, false})
                    .Build()
            },
            new object[]
            {
                4, // 1 * 2 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("ParallelHash")  // 1
                    .WithXOF(new [] {true, false})
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
        }
    }
}

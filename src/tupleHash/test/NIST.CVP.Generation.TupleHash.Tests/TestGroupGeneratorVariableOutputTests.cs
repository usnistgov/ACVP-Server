using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorVariableOutputTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0, // 1 * 0 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 1 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            },
            new object[]
            {
                4, // 1 * 2 * 2
                new ParameterValidatorTests.ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate2TestGroupsForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorVariableOutput();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());

            var totalShakeModes = 0;
            var totalNotShakeModes = 0;
            foreach (var result in results)
            {
                if (result.XOF)
                {
                    totalShakeModes++;
                }
                else
                {
                    totalNotShakeModes++;
                }
            }
            Assert.AreEqual(totalShakeModes, totalNotShakeModes);
        }
    }
}

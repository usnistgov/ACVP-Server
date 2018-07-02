using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorVariableMacTests
    {
        private static object[] parameters =
       {
            new object[]
            {
                0, // 1 * 0 * 2
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { }) // 0
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            },
            new object[]
            {
                2, // 1 * 1 * 2
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 }) // 1
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            },
            new object[]
            {
                4, // 1 * 2 * 2
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { 128, 256 }) // 2
                    .WithAlgorithm("TupleHash")  // 1
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate2TestGroupsForEachCombinationOfModeAndDigestSize(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorVariableMac();
            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());

            var totalXOF = 0;
            var totalNotXOF = 0;
            foreach (var result in results)
            {
                if (result.XOF)
                {
                    totalXOF++;
                }
                else
                {
                    totalNotXOF++;
                }
            }
            Assert.AreEqual(totalXOF, totalNotXOF);
        }
    }
}

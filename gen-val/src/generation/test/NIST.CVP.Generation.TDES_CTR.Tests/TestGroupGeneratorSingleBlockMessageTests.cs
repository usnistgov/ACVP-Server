using NIST.CVP.Generation.TDES_CTR.v1_0;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.TDES_CTR.Tests
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
        public void ShouldCreateATestGroupForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorSingleBlockMessage();

            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

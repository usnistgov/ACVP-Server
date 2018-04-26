using System.Linq;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CFB8.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorKnownAnswerTestTests
    {
        private static object[] parameters = new object[]
        {
            new object[]
            {
                0, // 4 * 0 * 1
                new ParameterBuilder()
                    .WithKeyLen(new int[] { }) // 0
                    .WithMode(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                4, // 4 * 1 * 1
                new ParameterBuilder()
                    .WithKeyLen(new[] {128}) // 1
                    .WithMode(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                16, // 4 * 2 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192}) // 2
                    .WithMode(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            },
            new object[]
            {
                24, // 4 * 3 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192, 256}) // 3
                    .WithMode(new[] {"encrypt", "decrypt"}) // 2
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate4TestGroupsForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            TestGroupGeneratorKnownAnswerTests subject = new TestGroupGeneratorKnownAnswerTests();

            var results = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

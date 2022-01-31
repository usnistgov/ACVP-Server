using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CFB128.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CFB128
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
        public async Task ShouldCreate4TestGroupsForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            TestGroupGeneratorKnownAnswerTests subject = new TestGroupGeneratorKnownAnswerTests();

            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

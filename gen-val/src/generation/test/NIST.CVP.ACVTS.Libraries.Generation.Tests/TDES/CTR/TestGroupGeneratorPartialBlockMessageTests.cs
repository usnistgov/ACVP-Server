using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.TDES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TDES.CTR
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorPartialBlockMessageTests
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
                0, // 1 * 1 * 0
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithDataLen(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build()
            },
            new object[]
            {
                2, // 2 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithDataLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 64, 8)))
                    .Build()
            },
            new object[]
            {
                3, // 3 * 2
                new ParameterBuilder()
                    .WithKeyingOption(new[] {1, 2}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithDataLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 64)))
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfKeyLengthAndDirection(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGeneratorPartialBlockMessage();

            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}

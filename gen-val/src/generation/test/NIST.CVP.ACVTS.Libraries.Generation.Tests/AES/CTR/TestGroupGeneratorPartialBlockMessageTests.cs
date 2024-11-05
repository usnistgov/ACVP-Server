using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.AES_CTR.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.AES.CTR
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
                    .WithKeyLen(new int[] { }) // 0
                    .WithDirection(new[] {"encrypt"})  // 1
                    .Build()
            },
            new object[]
            {
                0, // 1 * 1 * 0
                new ParameterBuilder()
                    .WithKeyLen(new[] {128}) // 1
                    .WithDirection(new[] {"encrypt"})  // 1
                    .WithDataLen(new MathDomain().AddSegment(new ValueDomainSegment(128)))
                    .Build()
            },
            new object[]
            {
                4, // 2 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192}) // 2
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithDataLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 128, 8)))
                    .Build()
            },
            new object[]
            {
                6, // 3 * 2
                new ParameterBuilder()
                    .WithKeyLen(new[] {128, 192, 256}) // 3
                    .WithDirection(new[] {"encrypt", "decrypt"}) // 2
                    .WithDataLen(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 1, 128)))
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

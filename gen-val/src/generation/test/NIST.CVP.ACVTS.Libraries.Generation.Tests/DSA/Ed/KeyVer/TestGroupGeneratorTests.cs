using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.KeyVer
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCurves(new [] { "ed-25519" })
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCurves(new [] { "ed-448" })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurves(ParameterValidator.VALID_CURVES)
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfCurveAndSecret(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

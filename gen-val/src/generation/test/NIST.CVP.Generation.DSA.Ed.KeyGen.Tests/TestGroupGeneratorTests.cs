using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.EDDSA.v1_0.KeyGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.KeyGen.Tests
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
                    .WithSecretGenerationModes(new [] {"testing candidates" })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurves(new [] { "ed-448" })
                    .WithSecretGenerationModes(ParameterValidator.VALID_SECRET_GENERATION_MODES)
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCurves(ParameterValidator.VALID_CURVES)
                    .WithSecretGenerationModes(ParameterValidator.VALID_SECRET_GENERATION_MODES)
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

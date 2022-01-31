using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SRTP;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SRTP
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
                    .WithKeyLength(new [] {128})
                    .WithKdr(new[] {1})
                    .WithZeroKdr(false)
                    .Build()
            },
            new object[]
            {
                16,
                new ParameterBuilder()
                    .WithKeyLength(new [] {128, 192})
                    .WithKdr(new[] {1, 2, 3, 4, 5, 6, 7})
                    .WithZeroKdr(true)
                    .Build()
            },
            new object[]
            {
                78,
                new ParameterBuilder()
                    .WithKeyLength(ParameterValidator.VALID_AES_KEY_LENGTHS)
                    .WithKdr(ParameterValidator.VALID_KDR_EXPONENTS)
                    .WithZeroKdr(true)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombination(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

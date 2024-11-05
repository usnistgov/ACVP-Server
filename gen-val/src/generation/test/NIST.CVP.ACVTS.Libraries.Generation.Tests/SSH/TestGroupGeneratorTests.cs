using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.SSH;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SSH
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
                    .WithCipher(new [] {"aes-128"})
                    .WithHashAlg(new[] {"sha2-512"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithCipher(new[] {"tdes"})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                20,
                new ParameterBuilder()
                    .WithCipher(ParameterValidator.VALID_CIPHERS)
                    .WithHashAlg(ParameterValidator.VALID_HASH_ALGS)
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfVersionAndHash(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(results.Count(), Is.EqualTo(expectedGroupsCreated));
        }
    }
}

using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.TLS.Enums;
using NIST.CVP.ACVTS.Libraries.Generation.KDF_Components.v1_0.TLS.v1_0;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.TLS
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
                    .WithVersion(new [] {TlsModes.v10v11})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithVersion(new[] {TlsModes.v12})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithVersion(new[] {TlsModes.v10v11, TlsModes.v12})
                    .WithHashAlg(new[] {"sha2-256", "sha2-384", "sha2-512"})
                    .Build()
            }
        };
        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreateATestGroupForEachCombinationOfVersionAndHash(int expectedGroupsCreated, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var results = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroupsCreated, results.Count());
        }
    }
}

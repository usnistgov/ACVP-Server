using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Generation.DSA.v1_0.SigGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.DSA.FFC.SigGen.Tests
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
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224, new[] { "sha-1" })
                        })
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(3072, 256, new[] { "sha2-384", "sha2-512/256" })
                        })
                    .Build()
            },
            new object[]
            {
                3 * 7,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(2048, 256, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(3072, 256, ParameterValidator.VALID_HASH_ALGS)
                        })
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfModeLN(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetDsaDomainParametersAsync(It.IsAny<DsaDomainParametersParameters>()))
                .Returns(Task.FromResult(new DsaDomainParametersResult()));

            var subject = new TestGroupGenerator(oracleMock.Object, false);
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

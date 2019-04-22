using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
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
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224" }, new[] { "sha2-224" })
                        })
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" })
                        })
                    .Build()
            },
            new object[]
            {
                12 * 10,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                        })
                    .Build()
            },
            new object[]
            {
                2 + 2 + 2 + 1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224", "b-233" }, new[] { "sha2-512", "sha2-224" })
                        })
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfCurveHashAlg(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetEcdsaKeyAsync(It.IsAny<EcdsaKeyParameters>()))
                .Returns(Task.FromResult(new EcdsaKeyResult { Key = new EccKeyPair(new EccPoint(1, 2), 3) }));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

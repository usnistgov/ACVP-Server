using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.KeyGen
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorGTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224)
                        })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224),
                            ParameterBuilder.GetCapabilityWith(3072, 256)
                        })
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(2048, 224),
                            ParameterBuilder.GetCapabilityWith(2048, 256),
                            ParameterBuilder.GetCapabilityWith(3072, 256)
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
                .Setup(s => s.GetDsaKeyAsync(It.IsAny<DsaKeyParameters>()))
                .Returns(Task.FromResult(new DsaKeyResult { Key = new FfcKeyPair() }));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(result.Count(), Is.EqualTo(expectedGroups));
        }
    }
}

using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.DSA.v1_0.KeyGen;

namespace NIST.CVP.Generation.DSA.FFC.KeyGen.Tests
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
        public void ShouldCreate1TestGroupForEachCombinationOfModeLN(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetDsaKeyAsync(It.IsAny<DsaKeyParameters>()))
                .Returns(Task.FromResult(new DsaKeyResult { Key = new FfcKeyPair() }));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

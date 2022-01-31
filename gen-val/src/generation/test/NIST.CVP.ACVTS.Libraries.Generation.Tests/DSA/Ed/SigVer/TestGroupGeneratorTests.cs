using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigVer
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
                    .WithCurve(new [] {"ed-25519"})
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurve(new [] {"ed-25519", "ed-448"})
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurve(ParameterValidator.VALID_CURVES)
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurve(new [] {"ed-25519"})
                    .WithPreHash(true)
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCurve(new [] {"ed-25519", "ed-448"})
                    .WithPreHash(true)
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCurve(ParameterValidator.VALID_CURVES)
                    .WithPreHash(true)
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithCurve(new [] {"ed-25519"})
                    .WithPreHash(true)
                    .WithPure(false)
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurve(new [] {"ed-25519", "ed-448"})
                    .WithPreHash(true)
                    .WithPure(false)
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurve(ParameterValidator.VALID_CURVES)
                    .WithPreHash(true)
                    .WithPure(false)
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfCurveHashAlg(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetEddsaKeyAsync(It.IsAny<EddsaKeyParameters>()))
                .Returns(Task.FromResult(new EddsaKeyResult { Key = new EdKeyPair(new BitString("ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf")) }));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Math;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Generation.EDDSA.v1_0.SigGen;

namespace NIST.CVP.Generation.DSA.Ed.SigGen.Tests
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
        public void ShouldCreate1TestGroupForEachCombinationOfCurveHashAlg(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetEddsaKeyAsync(It.IsAny<EddsaKeyParameters>()))
                .Returns(Task.FromResult(new EddsaKeyResult { Key = new EdKeyPair(new BitString("ec172b93ad5e563bf4932c70e1245034c35467ef2efd4d64ebf819683467e2bf")) }));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

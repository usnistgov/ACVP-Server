using Moq;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupFactoryGeneratedDataTestTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss"})
                    .WithModuli(new [] {2048})
                    .WithHashAlgs(new [] {"SHA-1"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss", "ansx9.31"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pkcs1v1.5"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-256", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
            new object[]
            {
                63,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigVerModes(new [] {"pss", "pkcs1v1.5", "ansx9.31"})
                    .WithModuli(new [] {1024, 2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate3TestGroupsForEachCombinationOfModeModuloAndHashAlg(int expectedGroups, Parameters parameters)
        {
            var oracleMock = new Mock<IOracle>();
            oracleMock
                .Setup(s => s.GetRsaKeyAsync(It.IsAny<RsaKeyParameters>()))
                .Returns(Task.FromResult(new RsaKeyResult()));

            var subject = new TestGroupGenerator(oracleMock.Object);
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups * 3, result.Count());
        }
    }
}

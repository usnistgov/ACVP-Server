using System.Linq;
using System.Threading.Tasks;
using Moq;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.RSA.Keys;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.SigGen
{
    [TestFixture, UnitTest]
    public class TestGroupFactoryGeneratedDataTestTests
    {
        private TestGroupGenerator _subject;

        [SetUp]
        public void SetUp()
        {
            var oracle = new Mock<IOracle>();
            oracle
                .Setup(s => s.GetRsaKeyAsync(It.IsAny<RsaKeyParameters>()))
                .Returns(Task.FromResult(new RsaKeyResult { Key = new KeyPair() }));

            _subject = new TestGroupGenerator(oracle.Object, false);
        }

        private static object[] parameters =
        {
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigGenModes(new [] {"pss"})
                    .WithModuli(new [] {2048})
                    .WithHashAlgs(new [] {"SHA-1"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigGenModes(new [] {"pss", "ansx9.31"})
                    .WithModuli(new [] {2048, 3072})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224"})
                    .Build()
            },
            new object[]
            {
                12,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigGenModes(new [] {"pkcs1v1.5"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-256", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
            new object[]
            {
                63,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithSigGenModes(new [] {"pss", "pkcs1v1.5", "ansx9.31"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            },
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfModeModuliAndHashAlg(int expectedGroups, Parameters parameters)
        {
            var result = await _subject.BuildTestGroupsAsync(parameters);
            Assert.That(result.Count(), Is.EqualTo(expectedGroups));
        }
    }
}

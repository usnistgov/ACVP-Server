using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.RSA.KeyGen
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorAftTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new[] {"probable"})           // Wrong mode
                    .WithModuli(new[] {2048, 4096})
                    .WithPrimeTests(new[] {"2pow100"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"provable", "provableWithProvableAux"})
                    .WithModuli(new [] {2048, 4096})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"probableWithProbableAux"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"2pow100"})
                    .Build()
            },
            new object[]
            {
                12,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"probableWithProvableAux"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"2pow100", "2powSecStr"})
                    .WithHashAlgs(new [] {"SHA2-512", "SHA2-512/224"})
                    .Build()
            },
            new object[]
            {
                90,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"provable", "provableWithProvableAux", "probableWithProvableAux", "probableWithProbableAux"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"2pow100", "2powSecStr"})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public async Task ShouldCreate1TestGroupForEachCombinationOfParams(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGeneratorAft();
            var result = await subject.BuildTestGroupsAsync(parameters);
            Assert.That(result.Count(), Is.EqualTo(expectedGroups));
        }
    }
}

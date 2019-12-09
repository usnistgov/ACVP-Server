using System.Linq;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorGdtTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new[] {"provable"})       // Wrong mode
                    .WithModuli(new[] {2048, 4096})
                    .WithPrimeTests(new[] {"2pow100"})
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"probable"})
                    .WithModuli(new [] {2048})
                    .WithPrimeTests(new [] {"2powSecStr"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"probable"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"2pow100"})
                    .Build()
            },
            new object[]
            {
                6,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"probable"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"2pow100", "2powSecStr"})
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfModuliAndPrimeTest(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGeneratorGdt();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

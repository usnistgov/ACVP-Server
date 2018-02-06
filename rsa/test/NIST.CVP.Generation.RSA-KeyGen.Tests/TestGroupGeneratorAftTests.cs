using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
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
                    .WithKeyGenModes(new[] {"B.3.3"})           // Wrong mode
                    .WithModuli(new[] {2048, 4096})
                    .WithPrimeTests(new[] {"tblC2"})
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.2", "B.3.4"})
                    .WithModuli(new [] {2048, 4096})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.6"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"tblC2"})
                    .Build()
            },
            new object[]
            {
                12,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.5"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"tblC2", "tblC3"})
                    .WithHashAlgs(new [] {"SHA2-512", "SHA2-512/224"})
                    .Build()
            },
            new object[]
            {
                90,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.2", "B.3.4", "B.3.5", "B.3.6"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"tblC2", "tblC3"})
                    .WithHashAlgs(new [] {"SHA-1", "SHA2-224", "SHA2-256", "SHA2-384", "SHA2-512", "SHA2-512/224", "SHA2-512/256"})
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfParams(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGeneratorAft();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

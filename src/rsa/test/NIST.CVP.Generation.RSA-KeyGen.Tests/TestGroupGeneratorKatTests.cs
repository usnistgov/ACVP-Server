using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorKatTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                0,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new[] {"B.3.3"})
                    .WithPubExpMode("fixed")            // Wrong mode
                    .WithModuli(new[] {2048})
                    .WithPrimeTests(new[] {"tblC3"})
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.3"})
                    .WithModuli(new [] {2048})
                    .WithPrimeTests(new [] {"tblC3"})
                    .Build()
            },
            new object[]
            {
                3,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.3"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"tblC2"})
                    .Build()
            },
            new object[]
            {
                6,
                new ParameterValidatorTests.ParameterBuilder()
                    .WithKeyGenModes(new [] {"B.3.3"})
                    .WithModuli(new [] {2048, 3072, 4096})
                    .WithPrimeTests(new [] {"tblC2", "tblC3"})
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfModuliAndPrimeTest(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGeneratorKat();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

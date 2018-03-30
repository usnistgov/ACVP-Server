using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigGen.Tests
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
        public void ShouldCreate1TestGroupForEachCombinationOfModeModuliAndHashAlg(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGeneratorGeneratedDataTest();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

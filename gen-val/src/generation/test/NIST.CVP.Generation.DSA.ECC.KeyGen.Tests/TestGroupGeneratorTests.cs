using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyGen.Tests
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
                    .WithCurves(new [] { "p-224" })
                    .WithSecretGenerationModes(new [] {"testing candidates" })
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithCurves(new [] { "k-283" })
                    .WithSecretGenerationModes(ParameterValidator.VALID_SECRET_GENERATION_MODES)
                    .Build()
            },
            new object[]
            {
                8,
                new ParameterBuilder()
                    .WithCurves(new [] { "p-256", "b-233", "k-409", "k-571" })
                    .WithSecretGenerationModes(ParameterValidator.VALID_SECRET_GENERATION_MODES)
                    .Build()
            },
            new object[]
            {
                24,
                new ParameterBuilder()
                    .WithCurves(ParameterValidator.VALID_CURVES)
                    .WithSecretGenerationModes(ParameterValidator.VALID_SECRET_GENERATION_MODES)
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfCurveAndSecret(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

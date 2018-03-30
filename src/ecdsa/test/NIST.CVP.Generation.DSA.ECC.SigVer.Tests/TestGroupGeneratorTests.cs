using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigVer.Tests
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
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224" }, new[] { "sha2-224" })
                        })
                    .Build()
            },
            new object[]
            {
                4,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" })
                        })
                    .Build()
            },
            new object[]
            {
                15 * 7,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                        })
                    .Build()
            },
            new object[]
            {
                15 * 7,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                            ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
                        })
                    .Build()
            },
            new object[]
            {
                2 + 2 + 2 + 1,
                new ParameterBuilder()
                    .WithCapabilities(new []
                        {
                            ParameterBuilder.GetCapabilityWith(new[] { "b-233" }, new[] { "sha2-512", "sha2-512/224" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "k-571" }, new[] { "sha2-384", "sha2-512/256" }),
                            ParameterBuilder.GetCapabilityWith(new[] { "p-224", "b-233" }, new[] { "sha2-512", "sha2-224" })
                        })
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfCurveHashAlg(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var result = subject.BuildTestGroups(parameters);
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.SigVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.SigVer
{
    [TestFixture, UnitTest]
    public class TestGroupGeneratorTests
    {
        private static object[] parameters =
        {
            new object[]
            {
                3,
                new ParameterBuilder()
                    .WithCapabilities(ParameterBuilder.GetGeneralCapabilitiesWith(
                        new [] { "LMS_SHA256_M32_H5" },
                        new [] { "LMOTS_SHA256_N32_W1" }))
                    .Build()
            },
            new object[]
            {
                23,
                new ParameterBuilder()
                    .WithCapabilities(ParameterBuilder.GetGeneralCapabilitiesWith(
                        new [] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10" },
                        new [] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W2" }))
                    .Build()
            },
            new object[]
            {
                38,
                new ParameterBuilder()
                    .WithCapabilities(ParameterBuilder.GetGeneralCapabilitiesWith(
                        new [] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10" },
                        ParameterValidator.VALID_LMOTS_TYPES))
                    .Build()
            },
            new object[]
            {
                74,
                new ParameterBuilder()
                    .WithCapabilities(ParameterBuilder.GetGeneralCapabilitiesWith(
                        ParameterValidator.VALID_LMS_TYPES,
                        ParameterValidator.VALID_LMOTS_TYPES))
                    .Build()
            },
            new object[]
            {
                1,
                new ParameterBuilder()
                    .WithSpecificParameters(true)
                    .WithSpecificCapabilities(ParameterBuilder.GetSpecificCapabilitiesWith(
                        new []
                        {
                            new[] { "LMS_SHA256_M32_H5" }
                        },
                        new []
                        {
                            new[] { "LMOTS_SHA256_N32_W1" }
                        }))
                    .Build()
            },
            new object[]
            {
                2,
                new ParameterBuilder()
                    .WithSpecificParameters(true)
                    .WithSpecificCapabilities(ParameterBuilder.GetSpecificCapabilitiesWith(
                        new []
                        {
                            new[] { "LMS_SHA256_M32_H5" },
                            ParameterValidator.VALID_LMS_TYPES
                        },
                        new []
                        {
                            new[] { "LMOTS_SHA256_N32_W1" },
                            new[] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1" }
                        }))
                    .Build()
            }
        };

        [Test]
        [TestCaseSource(nameof(parameters))]
        public void ShouldCreate1TestGroupForEachCombinationOfLmsAndLmots(int expectedGroups, Parameters parameters)
        {
            var subject = new TestGroupGenerator();
            var result = subject.BuildTestGroupsAsync(parameters).Result;
            Assert.AreEqual(expectedGroups, result.Count());
        }
    }
}

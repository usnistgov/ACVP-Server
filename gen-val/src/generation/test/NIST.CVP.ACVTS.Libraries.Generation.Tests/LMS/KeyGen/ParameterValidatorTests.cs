using NIST.CVP.ACVTS.Libraries.Generation.LMS.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.LMS.KeyGen
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var parameterBuilder = new ParameterBuilder();
            var result = subject.Validate(parameterBuilder.Build());

            Assert.IsNull(result.ErrorMessage);
            Assert.IsTrue(result.Success);
        }

        #region LmsTypesTestCases

        private static object[] lmsCases =
        {
            new object[]
            {
                "test 1",
                new string[]
                {
                    "LMOTS_SHA256_N32_W1"
                },
                false
            },
            new object[]
            {
                "test 2",
                new string[]
                {
                    "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10", "LMS_SHA256_M32_H6"
                },
                false
            },
            new object[]
            {
                "test 3",
                new string[] { },
                false
            },
            new object[]
            {
                "test 4",
                new string[]
                {
                    "lmsSHA256_M32_H5"
                },
                false
            }
        };
        #endregion LmsTypesTestCases
        [Test]
        [TestCaseSource(nameof(lmsCases))]
        public void ShouldReturnErrorWithInvalidLmsTypes(string testLabel, string[] lmsTypes, bool shouldPass)
        {
            var subject = new ParameterValidator();
            var capabilities = ParameterBuilder.GetGeneralCapabilitiesWith(lmsTypes, ParameterValidator.VALID_LMOTS_TYPES);
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(capabilities)
                .Build());

            Assert.AreEqual(shouldPass, result.Success);
        }

        #region LmotsTypesTestCases
        private static object[] lmotsCases =
        {
            new string[] { },
            new string[]
            {
                "LMOTS_SHA256_N32_W3",
                "LMOTS_SHA256_N32_W1"
            },
            new string[]
            {
                "LMTOS_SHA256_N32_W1"
            },
            new string[]
            {
                "LMOTS_SHA512_N32_W1"
            }
        };
        #endregion LmotsTypesTestCases
        [Test]
        [TestCaseSource(nameof(lmotsCases))]
        public void ShouldReturnErrorWithInvalidLmotsTypes(string[] lmotsTypes)
        {
            var subject = new ParameterValidator();
            var capabilities = ParameterBuilder.GetGeneralCapabilitiesWith(ParameterValidator.VALID_LMS_TYPES, lmotsTypes);
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(capabilities)
                .Build());

            Assert.IsFalse(result.Success);
        }

        #region LmsSpecificTypesTestCases

        private static object[] lmsSpecificCases =
        {
            new object[]
            {
                "test 1",
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1" }
                },
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1" }
                },
                false
            },
            new object[]
            {
                "test 2",
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10", "LMS_SHA256_M32_H6" }
                },
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1" }
                },
                false
            },
            new object[]
            {
                "test 3",
                new string[][] { },
                new string[][] { },
                false
            },
            new object[]
            {
                "test 4",
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5" },
                    new string[] { "LMS_SHA256_M32_H10", "LMS_SHA256_M32_H6" },
                    new string[] { "LMS_SHA256_M32_H10" }
                },
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1" },
                    new string[] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W1" },
                    new string[] { "LMOTS_SHA256_N32_W1" }
                },
                false
            }
        };
        #endregion LmsSpecificTypesTestCases
        [Test]
        [TestCaseSource(nameof(lmsSpecificCases))]
        public void ShouldReturnErrorWithInvalidLmsTypesSpecific(string testLabel, string[][] lmsTypes, string[][] lmotsTypes, bool shouldPass)
        {
            var subject = new ParameterValidator();
            var specificCapabilities = ParameterBuilder.GetSpecificCapabilitiesWith(lmsTypes, lmotsTypes);
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithSpecificParameters(true)
                    .WithSpecificCapabilities(specificCapabilities)
                .Build());

            Assert.AreEqual(shouldPass, result.Success);
        }

        #region LmotsSpecificTypesTestCases

        private static object[] lmotsSpecificCases =
        {
            new object[]
            {
                "test 1",
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5" }
                },
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5" }
                },
                false
            },
            new object[]
            {
                "test 2",
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5", "LMS_SHA256_M32_H10", "LMS_SHA256_M32_H5" }
                },
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N3_W1", "LMOTS_SHA256_N32_W1" }
                },
                false
            },
            new object[]
            {
                "test 3",
                new string[][] { },
                new string[][] { },
                false
            },
            new object[]
            {
                "test 4",
                new string[][]
                {
                    new string[] { "LMS_SHA256_M32_H5" },
                    new string[] { "LMS_SHA256_M32_H10", "LMS_SHA256_M32_H5" },
                    new string[] { "LMS_SHA256_M32_H10" }
                },
                new string[][]
                {
                    new string[] { "LMOTS_SHA256_N32_W1" },
                    new string[] { "LMOTS_SHA256_N32_W1", "LMOTS_SHA256_N32_W7" },
                    new string[] { "LMOTS_SHA256_N32_W1" }
                },
                false
            }
        };
        #endregion LmsSpecificTypesTestCases
        [Test]
        [TestCaseSource(nameof(lmotsSpecificCases))]
        public void ShouldReturnErrorWithInvalidLmotsTypesSpecific(string testLabel, string[][] lmsTypes, string[][] lmotsTypes, bool shouldPass)
        {
            var subject = new ParameterValidator();
            var specificCapabilities = ParameterBuilder.GetSpecificCapabilitiesWith(lmsTypes, lmotsTypes);
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithSpecificParameters(true)
                    .WithSpecificCapabilities(specificCapabilities)
                .Build());

            Assert.AreEqual(shouldPass, result.Success);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private GeneralCapabilities _capabilities;
        private bool _specificParameters;
        private SpecificCapability[] _specificCapabilities;

        public ParameterBuilder()
        {
            _algorithm = "LMS";
            _mode = "KeyGen";
            _capabilities = new GeneralCapabilities
            {
                LmsTypes = new[] { "LMS_SHA256_M32_H5",
                    "LMS_SHA256_M32_H10",
                    "LMS_SHA256_M32_H15",
                    "LMS_SHA256_M32_H20",
                    "LMS_SHA256_M32_H25"},
                LmotsTypes = new[] { "LMOTS_SHA256_N32_W1",
                    "LMOTS_SHA256_N32_W2",
                    "LMOTS_SHA256_N32_W4",
                    "LMOTS_SHA256_N32_W8" }
            };
            _specificParameters = false;
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMode(string value)
        {
            _mode = value;
            return this;
        }

        public ParameterBuilder WithCapabilities(GeneralCapabilities value)
        {
            _capabilities = value;
            return this;
        }

        public ParameterBuilder WithSpecificParameters(bool value)
        {
            _specificParameters = value;
            return this;
        }

        public ParameterBuilder WithSpecificCapabilities(SpecificCapability[] value)
        {
            _specificCapabilities = value;
            return this;
        }

        public static GeneralCapabilities GetGeneralCapabilitiesWith(string[] lmsTypes, string[] lmotsTypes)
        {
            return new GeneralCapabilities
            {
                LmsTypes = lmsTypes,
                LmotsTypes = lmotsTypes
            };
        }

        public static SpecificCapability[] GetSpecificCapabilitiesWith(string[][] lmsTypes, string[][] lmotsTypes)
        {
            var result = new SpecificCapability[lmsTypes.Length];
            for (int i = 0; i < lmsTypes.Length; i++)
            {
                result[i] = new SpecificCapability
                {
                    Levels = new LmsLevelParameters[lmsTypes[i].Length]
                };
                for (int j = 0; j < lmsTypes[i].Length; j++)
                {
                    result[i].Levels[j] = new LmsLevelParameters
                    {
                        LmsType = lmsTypes[i][j],
                        LmotsType = lmotsTypes[i][j]
                    };
                }
            }
            return result;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Capabilities = _capabilities,
                Specific = _specificParameters,
                SpecificCapabilities = _specificCapabilities
            };
        }

    }
}

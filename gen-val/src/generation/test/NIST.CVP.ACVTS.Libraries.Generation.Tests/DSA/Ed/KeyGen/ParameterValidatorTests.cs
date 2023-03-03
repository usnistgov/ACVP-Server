using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.KeyGen
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

        #region CurveTestCases
        private static object[] curveCases =
        {
            new string[]
            {
                "ED-25518"
            },
            new string[]
            {
                "ED-25519", "ED-444"
            },
            new string[] { },
            new string[]
            {
                "p-233"
            },
            new string[]
            {
                "ed448", "ED-25519"
            }
        };
        #endregion CurveTestCases
        [Test]
        [TestCaseSource(nameof(curveCases))]
        public void ShouldReturnErrorWithInvalidCurves(string[] curves)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCurves(curves)
                .Build());

            Assert.IsFalse(result.Success);
        }

        #region SecretGenerationTestCases
        private static object[] secretCases =
        {
            new string[] { },
            new string[]
            {
                "usingextra bits",
                "testing candidates"
            },
            new string[]
            {
                "testing candidate"
            },
            new string[]
            {
                "extrabits"
            }
        };
        #endregion SecretGenerationTestCases
        [Test]
        [TestCaseSource(nameof(secretCases))]
        public void ShouldReturnErrorWithInvalidSecretGenerationModes(string[] secretModes)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithSecretGenerationModes(secretModes)
                .Build());

            Assert.IsFalse(result.Success);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _curves;
        private string[] _secretGenerationModes;

        public ParameterBuilder()
        {
            _algorithm = "EDDSA";
            _mode = "keyGen";
            _curves = new[] { "ED-25519", "ED-448" };
            _secretGenerationModes = new[] { "testing candidates", "extra bits" };
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

        public ParameterBuilder WithCurves(string[] value)
        {
            _curves = value;
            return this;
        }

        public ParameterBuilder WithSecretGenerationModes(string[] value)
        {
            _secretGenerationModes = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Curve = _curves,
                SecretGenerationMode = _secretGenerationModes
            };
        }
    }
}

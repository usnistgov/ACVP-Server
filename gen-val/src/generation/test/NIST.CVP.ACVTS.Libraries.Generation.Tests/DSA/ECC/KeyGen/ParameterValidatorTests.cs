using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyGen
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

            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Success, Is.True);
        }

        #region CurveTestCases

        private static object[] curveCases =
        {
            new object[]
            {
                "test 1",
                new string[]
                {
                    "P-192"
                },
                false
            },
            new object[]
            {
                "test 2",
                new string[]
                {
                    "p-192", "v-163", "k-571"
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
                    "p-233"
                },
                false
            },
            new object[]
            {
                "test 5",
                new string[]
                {
                    "p-192", "b-163", "k-163"
                },
                false
            }
        };
        #endregion CurveTestCases
        [Test]
        [TestCaseSource(nameof(curveCases))]
        public void ShouldReturnErrorWithInvalidCurves(string testLabel, string[] curves, bool shouldPass)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCurves(curves)
                .Build());

            Assert.That(result.Success, Is.EqualTo(shouldPass));
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

            Assert.That(result.Success, Is.False);
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
            _algorithm = "ECDSA";
            _mode = "keyGen";
            _curves = new[] { "P-224", "B-283", "K-409" };
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

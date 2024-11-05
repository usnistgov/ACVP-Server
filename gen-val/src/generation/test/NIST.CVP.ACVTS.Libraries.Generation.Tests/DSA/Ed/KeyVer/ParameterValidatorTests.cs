using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.KeyVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.KeyVer
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
            new string[]
            {
                "ED25519"
            },
            new string[]
            {
                "ED-449"
            },
            new string[] { },
            new string[]
            {
                "p-233"
            },
            new string[]
            {
                "ED-25519", "ED-448", ""
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

            Assert.That(result.Success, Is.False);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _curves;

        public ParameterBuilder()
        {
            _algorithm = "EDDSA";
            _mode = "keyVer";
            _curves = new[] { "ED-25519", "ED-448" };
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

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Curve = _curves,
            };
        }
    }
}

using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.KeyVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.KeyVer
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
                "P192"
            },
            new string[]
            {
                "p-192", "v-163", "k-571"
            },
            new string[] { },
            new string[]
            {
                "p-233"
            },
            new string[]
            {
                "p-192", "b-163", "k-163", ""
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
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private string[] _curves;

        public ParameterBuilder()
        {
            _algorithm = "ECDSA";
            _mode = "keyVer";
            _curves = new[] { "P-224", "B-283", "K-409" };
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

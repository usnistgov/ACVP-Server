using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.KeyVer.Tests
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
            _mode = "KeyVer";
            _curves = new[] { "p-224", "b-283", "k-409" };
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

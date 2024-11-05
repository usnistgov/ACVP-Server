using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.Ed.SigGen
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

        [Test]
        [TestCase(false, false, TestName = "ShouldReturnErrorWithInvalidPreHashPure - invalid")]
        public void ShouldReturnErrorWithInvalidPreHashPure(bool preHash, bool pure)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(preHash)
                    .WithPure(pure)
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public void ShouldReturnSuccessWithValidPreHashPure(bool preHash, bool pure)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(preHash)
                    .WithPure(pure)
                    .Build());

            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidCurve - invalid")]
        public void ShouldReturnErrorWithInvalidCurve(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCurve(strModes)
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(new object[] { "ED-25519", "ED-448" })]
        [TestCase(new object[] { "ED-25519" })]
        [TestCase(new object[] { "ED-448" })]
        public void ShouldReturnSuccessWithNewCapability(object[] curves)
        {
            var strCurve = curves.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCurve(strCurve)
                    .Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private bool _preHash;
        private bool _pure;
        private string[] _curve;

        public ParameterBuilder()
        {
            _algorithm = "EDDSA";
            _mode = "sigGen";
            _preHash = false;
            _pure = true;
            _curve = ParameterValidator.VALID_CURVES;
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

        public ParameterBuilder WithCurve(string[] value)
        {
            _curve = value;
            return this;
        }

        public ParameterBuilder WithPreHash(bool value)
        {
            _preHash = value;
            return this;
        }

        public ParameterBuilder WithPure(bool value)
        {
            _pure = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                PreHash = _preHash,
                Pure = _pure,
                Curve = _curve
            };
        }
    }
}

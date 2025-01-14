using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.EDDSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
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
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));
            var parameterBuilder = new ParameterBuilder().WithCurve(["ED-448"]).WithContextLength(contextLength);
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
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));

            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(preHash)
                    .WithPure(pure)
                    .WithContextLength(contextLength)
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
        [TestCase(new object[] { "ED-25519", "ED-448" }, true)]
        [TestCase(new object[] { "ED-25519" }, false)]
        [TestCase(new object[] { "ED-448" }, true)]
        public void ShouldReturnWithNewCapability(object[] curves, bool shouldPass)
        {
            var strCurve = curves.Select(v => (string)v).ToArray();
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPure(true)
                    .WithCurve(strCurve)
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, shouldPass ? Is.True : Is.False, result.ErrorMessage);
        }
        
        [Test]
        public void ShouldReturnValidWithCurveEd25519AndContextLengthProvidedWithPreHash()
        {
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(true)
                    .WithPure(true)
                    .WithCurve(["ED-25519"])
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void ShouldReturnInvalidWithCurveEd25519AndContextLengthProvidedWithoutPreHash()
        {
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(false)
                    .WithPure(true)
                    .WithCurve(["ED-25519"])
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void ShouldReturnValidWithCurveEd448AndContextLengthProvided()
        {
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPure(true)
                    .WithCurve(["ED-448"])
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void ShouldReturnValidWithCurveEd448AndContextLengthNotProvided()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPure(true)
                    .WithCurve(["ED-448"])
                    .Build());

            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        public void ShouldReturnValidWithContextLengthProvidedAndPreHash()
        {
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 255));
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(true)
                    .WithCurve(["ED-448"])
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, Is.True);
        }
        
        [Test]
        public void ShouldReturnInvalidWithContextLengthNotProvidedAndPreHash()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPreHash(true)
                    .WithCurve(["ED-448"])
                    .Build());

            Assert.That(result.Success, Is.False);
        }
        
        [Test]
        [TestCase(0, 255, true)]
        [TestCase(0, 256, false)]
        [TestCase(-1, 255, false)]
        public void ShouldAssertValidContextLength(int min, int max, bool shouldPass)
        {
            var contextLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), min, max));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithContextLength(contextLength)
                    .Build());

            Assert.That(result.Success, shouldPass ? Is.True : Is.False, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private bool _preHash;
        private bool _pure;
        private string[] _curve;
        private MathDomain _contextLength;

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

        public ParameterBuilder WithContextLength(MathDomain value)
        {
            _contextLength = value;
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
                Curve = _curve,
                ContextLength = _contextLength
            };
        }
    }
}

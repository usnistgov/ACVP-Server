using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.ECDSA.v1_0.SigVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.ECC.SigVer
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
        public void ShouldNotAllow106ConformanceAndPreHash()
        {
            var subject = new ParameterValidator();
            var parameters = new ParameterBuilder()
                .WithComponent(true)
                .WithConformances(new[] { "SP800-106" })
                .Build();

            var result = subject.Validate(parameters);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - invalid")]
        [TestCase(new object[] { "SHA2-256", "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid")]
        [TestCase(new object[] { "SHA2-512/224", "SHA2-384", null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid with null")]
        public void ShouldReturnErrorWithInvalidHashAlg(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(ParameterValidator.VALID_CURVES, strModes)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidCurve - invalid")]
        [TestCase(new object[] { "P-224", "notValid" }, TestName = "ShouldReturnErrorWithInvalidCurve - partially valid")]
        [TestCase(new object[] { "P-256", "B-409", null }, TestName = "ShouldReturnErrorWithInvalidCurve - partially valid with null")]
        public void ShouldReturnErrorWithInvalidCurve(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(strModes, ParameterValidator.VALID_HASH_ALGS)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(new object[] { "P-224", "K-283" }, new object[] { "SHA2-512/224" })]
        [TestCase(new object[] { "P-224", "B-571" }, new object[] { "SHA2-512" })]
        [TestCase(new object[] { "P-521", "K-233" }, new object[] { "SHA2-224" })]
        public void ShouldReturnSuccessWithNewCapability(object[] curves, object[] hashAlgs)
        {
            var strCurve = curves.Select(v => (string)v).ToArray();
            var strHash = hashAlgs.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(strCurve, strHash)
                    })
                    .Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;
        private bool _component;
        private string[] _conformances;

        public ParameterBuilder()
        {
            _algorithm = "ECDSA";
            _mode = "sigVer";
            _capabilities = new[]
            {
                GetCapabilityWith(ParameterValidator.VALID_CURVES, ParameterValidator.VALID_HASH_ALGS),
            };
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

        public ParameterBuilder WithComponent(bool value)
        {
            _component = value;
            return this;
        }

        public ParameterBuilder WithConformances(string[] value)
        {
            _conformances = value;
            return this;
        }

        public ParameterBuilder WithCapabilities(Capability[] value)
        {
            _capabilities = value;
            return this;
        }

        public static Capability GetCapabilityWith(string[] curves, string[] hashAlgs)
        {
            return new Capability
            {
                Curve = curves,
                HashAlg = hashAlgs
            };
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                Capabilities = _capabilities,
                Component = _component,
                Conformances = _conformances
            };
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.Ed.SigVer.Tests
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

            Assert.IsFalse(result.Success);
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

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - invalid")]
        public void ShouldReturnErrorWithInvalidCurve(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(strModes)
                    })
                    .Build());

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(new object[] { "ed-25519", "ed-448" })]
        [TestCase(new object[] { "ed-25519"  })]
        [TestCase(new object[] { "ed-448" })]
        public void ShouldReturnSuccessWithNewCapability(object[] curves)
        {
            var strCurve = curves.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(strCurve)
                    })
                    .Build());

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private bool _preHash;
        private bool _pure;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            _algorithm = "EDDSA";
            _mode = "SigGen";
            _preHash = false;
            _pure = true;
            _capabilities = new[]
            {
                GetCapabilityWith(ParameterValidator.VALID_CURVES),
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

        public ParameterBuilder WithCapabilities(Capability[] value)
        {
            _capabilities = value;
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

        public static Capability GetCapabilityWith(string[] curves)
        {
            return new Capability
            {
                Curve = curves
            };
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Mode = _mode,
                PreHash = _preHash,
                Pure = _pure,
                Capabilities = _capabilities
            };
        }
    }
}

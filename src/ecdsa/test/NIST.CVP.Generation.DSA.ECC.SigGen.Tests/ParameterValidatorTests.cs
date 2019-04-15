using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NIST.CVP.Generation.ECDSA.v1_0.SigGen;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.DSA.ECC.SigGen.Tests
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
        //[TestCase(new object[] { null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - null")]
        //[TestCase(new object[] { }, TestName = "ShouldReturnErrorWithInvalidHashAlg - empty")]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - invalid")]
        [TestCase(new object[] { "sha2-256", "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid")]
        [TestCase(new object[] { "sha2-512/224", "sha2-384", null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid with null")]
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

            Assert.IsFalse(result.Success);
        }

        [Test]
        //[TestCase(new object[] { null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - null")]
        //[TestCase(new object[] { }, TestName = "ShouldReturnErrorWithInvalidHashAlg - empty")]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - invalid")]
        [TestCase(new object[] { "p-224", "notValid" }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid")]
        [TestCase(new object[] { "p-256", "b-409", null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - partially valid with null")]
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

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(new object[] { "p-224", "k-283" }, new object[] { "sha2-512/224" })]
        [TestCase(new object[] { "p-224", "b-571" }, new object[] { "sha2-512" })]
        [TestCase(new object[] { "p-521", "k-233" }, new object[] { "sha2-224" })]
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

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private string _mode;
        private Capability[] _capabilities;

        public ParameterBuilder()
        {
            _algorithm = "ECDSA";
            _mode = "SigGen";
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
                Capabilities = _capabilities
            };
        }
    }
}

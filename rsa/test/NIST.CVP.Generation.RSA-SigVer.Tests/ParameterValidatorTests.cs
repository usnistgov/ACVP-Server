using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NIST.CVP.Generation.RSA_SigVer.Tests
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
        [TestCase("null", new object[] { null })]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "pss", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "pkcs1v13", null })]
        public void ShouldReturnErrorWithInvalidSigVerMode(string label, object[] mode)
        {
            var strAlgs = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithSigVerModes(strAlgs)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase("null", new object[] { null })]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "SHA-224", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "HA-512/256", null })]
        public void ShouldReturnErrorWithInvalidHashAlgorithm(string label, object[] hashAlg)
        {
            var strAlgs = hashAlg.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlgs(strAlgs)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid", new object[] { 1243 })]
        [TestCase("Partially valid", new object[] { 2048, 9001 })]
        public void ShouldReturnErrorWithInvalidModulo(string label, object[] modulo)
        {
            var mods = modulo.Select(v => (int)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithModuli(mods)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase("empty", "")]
        [TestCase("small", "03")]
        [TestCase("even", "ABCDEF02")]
        [TestCase("not hex", "BEEFFACEX")]
        public void ShouldReturnErrorWithInvalidEValue(string label, string value)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithEValue(value)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        public void ShouldReturnSuccessWithNewSigVerModes()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithSigVerModes(new[] { "ansx9.31", "pss" })
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewHashAlgs()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithHashAlgs(new[] { "sha-384", "sha-512" })
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        public void ShouldReturnSuccessWithNewModulo()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithModuli(new[] { 1024, 3072 })
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string _mode;
            private string[] _sigVerModes;
            private string[] _hashAlgs;
            private int[] _moduli;
            private string _eValue;

            public ParameterBuilder()
            {
                _algorithm = "RSA";
                _mode = "SigVer";
                _sigVerModes = new[] { "ansx9.31", "pss" };
                _hashAlgs = new[] { "sha-1", "sha-256" };
                _moduli = new[] { 2048 };
                _eValue = "010001";
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithSigVerModes(string[] value)
            {
                _sigVerModes = value;
                return this;
            }

            public ParameterBuilder WithHashAlgs(string[] value)
            {
                _hashAlgs = value;
                return this;
            }

            public ParameterBuilder WithModuli(int[] value)
            {
                _moduli = value;
                return this;
            }

            public ParameterBuilder WithEValue(string value)
            {
                _eValue = value;
                return this;
            }

            public Parameters Build()
            {
                var hashPairs = new HashPair[_hashAlgs.Length];
                for (var i = 0; i < hashPairs.Length; i++)
                {
                    hashPairs[i] = new HashPair
                    {
                        HashAlg = _hashAlgs[i],
                        SaltLen = (i + 1)
                    };
                }

                var capabilities = new SigCapability[_moduli.Length];
                for (var i = 0; i < capabilities.Length; i++)
                {
                    capabilities[i] = new SigCapability
                    {
                        Modulo = _moduli[i],
                        HashPairs = hashPairs
                    };
                }

                return new Parameters
                {
                    Algorithm = _algorithm,
                    Mode = _mode,
                    SigVerModes = _sigVerModes,
                    Capabilities = capabilities,
                    PubExpMode = "fixed",
                    FixedPubExpValue = _eValue
                };
            }
        }
    }
}

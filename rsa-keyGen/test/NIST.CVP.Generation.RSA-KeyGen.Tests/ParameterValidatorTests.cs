using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.RSA_KeyGen.Tests
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
        [TestCase("Partially valid", new object[] { "B.3.2", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "B.3.", null })]
        public void ShouldReturnErrorWithInvalidKeyGenMode(string label, object[] mode)
        {
            var strAlgs = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(strAlgs)
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
        public void ShouldReturnSuccessWithNewKeyGenModes()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {"B.3.2", "B.3.3"})
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
                    .WithHashAlgs(new [] {"SHA-384", "SHA-512"})
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewModulo()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithModuli(new [] {4096})
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("")]
        [TestCase("010001")]
        [TestCase("BEEFFACE")]
        public void ShouldValidateHex(string hex)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new Parameters
            {
                Moduli = new [] {2048},
                HashAlgs = new [] {"SHA-1"},
                KeyGenModes = new [] {"B.3.2"},
                PubExpMode = "fixed",
                FixedPubExp = hex
            });

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase("GG")]
        [TestCase("0192809ajsf0j3")]
        [TestCase(null)]
        public void ShouldNotValidateInvalidHex(string hex)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(new Parameters
            {
                Moduli = new[] { 2048 },
                HashAlgs = new[] { "SHA-1" },
                KeyGenModes = new[] { "B.3.2" },
                PubExpMode = "fixed",
                FixedPubExp = hex
            });

            Assert.IsFalse(result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string[] _keyGenModes;
            private string[] _hashAlgs;
            private int[] _moduli;
            private string[] _primeTests;
            private string _pubExpMode;

            public ParameterBuilder()
            {
                _algorithm = "RSA-KeyGen";
                _keyGenModes = new[] {"B.3.4", "B.3.6"};
                _moduli = new[] {2048, 3072};
                _hashAlgs = new[] {"SHA-1", "SHA-256"};
                _primeTests = new[] {"tblC2", "tblC3"};
                _pubExpMode = "random";
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithKeyGenModes(string[] value)
            {
                _keyGenModes = value;
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

            public ParameterBuilder WithPrimeTests(string[] value)
            {
                _primeTests = value;
                return this;
            }

            public ParameterBuilder WithPubExpMode(string value)
            {
                _pubExpMode = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    KeyGenModes = _keyGenModes,
                    HashAlgs = _hashAlgs,
                    Moduli = _moduli,
                    PubExpMode = _pubExpMode,
                    PrimeTests = _primeTests
                };
            }
        }
    }
}

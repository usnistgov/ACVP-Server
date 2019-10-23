using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Common.Helpers;
using NIST.CVP.Crypto.Common.Asymmetric.RSA.Enums;
using NIST.CVP.Generation.RSA.v1_0.KeyGen;
using NIST.CVP.Math;
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
        [TestCase("Invalid", new object[] { "invalid" })]
        [TestCase("Partially valid", new object[] { "provable", "invalid" })]
        [TestCase("Partially valid with null", new object[] { "invalid", null })]
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
        [TestCase("Invalid", new object[] { "invalid" })]
        [TestCase("Partially valid", new object[] { "sha2-224", "invalid" })]
        [TestCase("Partially valid with null", new object[] { "HA2-512/256", null })]
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
        [TestCase("03")]
        [TestCase("")]
        [TestCase("00")]
        [TestCase("1234")]
        public void ShouldReturnErrorWithInvalidEValue(string fixedValue)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPubExpMode("fixed")
                    .WithFixedPubExp(fixedValue)
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewKeyGenModes()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {"provable", "probable"})
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
                    .WithHashAlgs(new [] {"sha2-384", "sha2-512"})
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
                    .WithModuli(new [] {3072})
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("010001")]
        [TestCase("BEEFFACD")]
        public void ShouldValidateOddHex(string hex)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithPubExpMode("fixed")
                    .WithFixedPubExp(hex)
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase("provable")]
        [TestCase("provableWithProvableAux")]
        [TestCase("probableWithProvableAux")]
        public void ShouldCheckHashAlgForCertainModes(string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {mode})
                    .WithHashAlgs(null)
                    .WithPrimeTests(new[] {"2powSecStr"})
                    .Build()
            );

            Assert.IsFalse(result.Success, "Success check");
            Assert.IsTrue(result.ErrorMessage.Contains("Hash Alg"), "Contains check");
        }

        [Test]
        [TestCase("probable")]
        [TestCase("probableWithProbableAux")]
        public void ShouldNotCheckHashAlgForCertainModes(string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {mode})
                    .WithHashAlgs(null)
                    .WithPrimeTests(new[] {"2powSecStr"})
                    .Build()
            );

            Assert.IsTrue(result.Success, "Success check");
        }

        [Test]
        [TestCase("probable")]
        [TestCase("probableWithProvableAux")]
        [TestCase("probableWithProbableAux")]
        public void ShouldCheckPrimeTestForCertainModes(string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {mode})
                    .WithHashAlgs(new[] {"sha2-512"})
                    .WithPrimeTests(new [] {"none"})
                    .Build()
            );

            Assert.IsFalse(result.Success, "Success check");
        }

        [Test]
        [TestCase("provable")]
        [TestCase("provableWithProvableAux")]
        public void ShouldNotCheckPrimeTestForCertainModes(string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithKeyGenModes(new[] {mode})
                    .WithHashAlgs(new[] {"sha2-512"})
                    .WithPrimeTests(new [] {"none"})
                    .Build()
            );

            Assert.IsTrue(result.Success, "Success check");
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string _mode;
            private string[] _keyGenModes;
            private string[] _hashAlgs;
            private int[] _moduli;
            private string[] _primeTests;
            private string _pubExpMode;
            private string _fixedPubExp;
            private string _keyFormat;

            public ParameterBuilder()
            {
                _algorithm = "RSA";
                _mode = "KeyGen";
                _keyGenModes = new[] {"provableWithProvableAux", "probableWithProbableAux"};
                _moduli = new[] {2048};
                _hashAlgs = new[] {"sha-1", "sha2-256"};
                _primeTests = new[] {"2powSecStr", "2pow100"};
                _pubExpMode = "random";
                _fixedPubExp = "";
                _keyFormat = "standard";
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

            public ParameterBuilder WithFixedPubExp(string value)
            {
                _fixedPubExp = value;
                return this;
            }

            public Parameters Build()
            {
                var caps = new Capability[_moduli.Length];
                for (var i = 0; i < caps.Length; i++)
                {
                    caps[i] = new Capability
                    {
                        Modulo = _moduli[i],
                        HashAlgs = _hashAlgs,
                        PrimeTests = _primeTests.Select(pt => EnumHelpers.GetEnumFromEnumDescription<PrimeTestModes>(pt, false)).ToArray()
                    };
                }

                var algSpecs = new AlgSpec[_keyGenModes.Length];
                for (var i = 0; i < algSpecs.Length; i++)
                {
                    algSpecs[i] = new AlgSpec
                    {
                        RandPQ = EnumHelpers.GetEnumFromEnumDescription<PrimeGenModes>(_keyGenModes[i], false),
                        Capabilities = caps
                    };
                }

                return new Parameters
                {
                    Algorithm = _algorithm,
                    Mode = _mode,
                    PubExpMode = EnumHelpers.GetEnumFromEnumDescription<PublicExponentModes>(_pubExpMode, false),
                    FixedPubExp = new BitString(_fixedPubExp),
                    AlgSpecs = algSpecs,
                    KeyFormat = EnumHelpers.GetEnumFromEnumDescription<PrivateKeyModes>(_keyFormat, false)
                };
            }
        }
    }
}

﻿using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.PqgVer;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.PQGVer
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
        //[TestCase(new object[] { null }, TestName = "ShouldReturnErrorWithInvalidPQMode - null")]
        //[TestCase(new object[] { }, TestName = "ShouldReturnErrorWithInvalidPQMode - empty")]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidPQMode - invalid")]
        [TestCase(new object[] { "probable", "notValid" }, TestName = "ShouldReturnErrorWithInvalidPQMode - partially valid")]
        [TestCase(new object[] { "provable", null }, TestName = "ShouldReturnErrorWithInvalidPQMode - partially valid with null")]
        public void ShouldReturnErrorWithInvalidPQMode(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(1024, 160, strModes, ParameterValidator.VALID_G_MODES, ParameterValidator.VALID_HASH_ALGS)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        //[TestCase(new object[] { null }, TestName = "ShouldReturnErrorWithInvalidGMode - null")]
        //[TestCase(new object[] { }, TestName = "ShouldReturnErrorWithInvalidGMode - empty")]
        [TestCase(new object[] { "notValid" }, TestName = "ShouldReturnErrorWithInvalidGMode - invalid")]
        [TestCase(new object[] { "unverifiable", "notValid" }, TestName = "ShouldReturnErrorWithInvalidGMode - partially valid")]
        [TestCase(new object[] { "canonical", null }, TestName = "ShouldReturnErrorWithInvalidGMode - partially valid with null")]
        public void ShouldReturnErrorWithInvalidGMode(object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(1024, 160, ParameterValidator.VALID_PQ_MODES, strModes, ParameterValidator.VALID_HASH_ALGS)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        //[TestCase(new object[] { null }, TestName = "ShouldReturnErrorWithInvalidHashAlg - null")]
        //[TestCase(new object[] { }, TestName = "ShouldReturnErrorWithInvalidHashAlg - empty")]
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
                        ParameterBuilder.GetCapabilityWith(2048, 224, ParameterValidator.VALID_PQ_MODES, ParameterValidator.VALID_G_MODES, strModes)
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(2048, 160)]
        [TestCase(2049, 224)]
        [TestCase(3072, 257)]
        [TestCase(1024, 224)]
        [TestCase(1, 2)]
        public void ShouldReturnErrorWithInvalidLNPair(int L, int N)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(L, N, ParameterValidator.VALID_PQ_MODES, ParameterValidator.VALID_G_MODES, new[] { "SHA-1" })
                    })
                    .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldReturnSuccessWithNewCapability()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(3072, 256, ParameterValidator.VALID_PQ_MODES, ParameterValidator.VALID_G_MODES, new[] { "SHA2-256", "SHA2-384", "SHA2-512" })
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

        public ParameterBuilder()
        {
            _algorithm = "DSA";
            _mode = "pqgVer";
            _capabilities = new[] { GetCapabilityWith(2048, 224, new[] { "probable" }, new[] { "unverifiable" }, new[] { "SHA2-256" }) };
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

        public static Capability GetCapabilityWith(int l, int n, string[] pqModes, string[] gModes, string[] hashAlgs)
        {
            return new Capability
            {
                L = l,
                N = n,
                PQGen = pqModes,
                GGen = gModes,
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

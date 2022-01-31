using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.DSA.v1_0.SigGen;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.DSA.FFC.SigGen
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
        public void ShouldReturnNoErrorsWithValidParametersIncludingConformances()
        {
            var subject = new ParameterValidator();
            var parameterBuilder = new ParameterBuilder()
                .WithConformances(new[] { "SP800-106" });
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
                        ParameterBuilder.GetCapabilityWith(2048, 224, strModes)
                    })
                    .Build());

            Assert.IsFalse(result.Success);
        }

        [Test]
        [TestCase(2048, 160)]
        [TestCase(2049, 224)]
        [TestCase(3072, 257)]
        [TestCase(1024, 160)]
        [TestCase(1, 2)]
        public void ShouldReturnErrorWithInvalidLNPair(int L, int N)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(L, N, ParameterValidator.VALID_HASH_ALGS)
                    })
                    .Build());

            Assert.IsFalse(result.Success);
        }

        private static IEnumerable<object> _badConformances = new List<object>()
        {
            new object[]
            {
                new[] {"invalid"}
            },
            new object[]
            {
                new[] {"SP800-106", "invalid"}
            }
        };

        [Test]
        [TestCaseSource(nameof(_badConformances))]
        public void ShouldReturnErrorWithInvalidConformances(string[] conformances)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithConformances(conformances)
                    .Build());

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(2048, 224)]
        [TestCase(2048, 256)]
        [TestCase(3072, 256)]
        public void ShouldReturnSuccessWithNewCapability(int L, int N)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithCapabilities(new[]
                    {
                        ParameterBuilder.GetCapabilityWith(L, N, ParameterValidator.VALID_HASH_ALGS)
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
        private string[] _conformances;

        public ParameterBuilder()
        {
            _algorithm = "DSA";
            _mode = "SigGen";
            _capabilities = new[]
            {
                GetCapabilityWith(2048, 224, ParameterValidator.VALID_HASH_ALGS),
                GetCapabilityWith(2048, 256, ParameterValidator.VALID_HASH_ALGS),
                GetCapabilityWith(3072, 256, ParameterValidator.VALID_HASH_ALGS)
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

        public ParameterBuilder WithConformances(string[] value)
        {
            _conformances = value;
            return this;
        }

        public static Capability GetCapabilityWith(int l, int n, string[] hashAlgs)
        {
            return new Capability
            {
                L = l,
                N = n,
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
                Conformances = _conformances
            };
        }
    }
}

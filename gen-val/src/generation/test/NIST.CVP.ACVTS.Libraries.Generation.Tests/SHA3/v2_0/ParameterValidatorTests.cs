using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v2_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v2_0
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
        [TestCase("Invalid valid", "SHA-1")]
        [TestCase("Partially valid", "SHA2-224")]
        public void ShouldReturnErrorWithInvalidAlgorithm(string label, string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(mode)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase(16, 65537, 1, "SHA3-224")]
        [TestCase(-1, 65536, 1, "SHA3-224")]
        [TestCase(16, 65537, 1, "SHA3-512")]
        [TestCase(-1, 65536, 1, "SHA3-512")]
        public void ShouldReturnErrorWithInvalidMessageLength(int min, int max, int inc, string algo)
        {
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, min, max, inc));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithMessageLength(messageLength)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(16, 65536, 8, "SHA3-224")]
        [TestCase(224, 65536, 1, "SHA3-224")]
        [TestCase(1, 65535, 1, "SHA3-512")]
        [TestCase(0, 1536, 8, "SHA3-512")]
        public void ShouldReturnWithValidMessageLength(int min, int max, int inc, string algo)
        {
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, min, max, inc));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithMessageLength(messageLength)
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(new[] { 1, 2, 3, 4 })]
        [TestCase(new[] { 5 })]
        public void ShouldReturnErrorWithInvalidLdt(int[] ldt)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithLdt(ldt)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string _revision;
            public List<string> _digests;
            private MathDomain _messageLength;
            private int[] _ldt;

            public ParameterBuilder()
            {
                _algorithm = "SHA3-224";
                _revision = "2.0";
                _digests = new List<string> { "256" };

                _ldt = new[] { 1 };
                _messageLength = new MathDomain();
                _messageLength.AddSegment(new RangeDomainSegment(null, 16, 65536));
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithRevision(string value)
            {
                _revision = value;
                return this;
            }

            public ParameterBuilder WithMessageLength(MathDomain value)
            {
                _messageLength = value;
                return this;
            }

            public ParameterBuilder WithLdt(int[] value)
            {
                _ldt = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Revision = _revision,
                    DigestSizes = _digests,
                    MessageLength = _messageLength,
                    PerformLargeDataTest = _ldt
                };
            }
        }
    }
}

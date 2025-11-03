using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.SHAKE.FIPS202;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHAKE.FIPS202
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
        [TestCase("Completely invalid", "SHA-2")]
        [TestCase("Partially invalid", "SHAKE-128v2")]
        public void ShouldReturnErrorWithInvalidAlgorithm(string label, string mode)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(mode)
                    .Build()
            );

            Assert.That(result.Success, Is.False, label);
        }

        [Test]
        [TestCase(4, 65537, 1, "SHAKE-128")]
        [TestCase(-1, 65536, 1, "SHAKE-128")]
        [TestCase(7, 65537, 1, "SHAKE-256")]
        [TestCase(-1, 65536, 1, "SHAKE-256")]
        public void ShouldReturnErrorWithInvalidMessageLength(int min, int max, int inc, string algo)
        {
            Random800_90 rand = new Random800_90();
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, min, max, inc));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithMessageLength(messageLength)
                    .WithOutputLength(new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 512, 8)))
                    .Build()
            );

            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        [TestCase(16, 65536, 8, "SHAKE-128")]
        [TestCase(224, 65536, 1, "SHAKE-128")]
        [TestCase(1, 65535, 1, "SHAKE-256")]
        [TestCase(0, 1536, 8, "SHAKE-256")]
        public void ShouldReturnWithValidMessageLength(int min, int max, int inc, string algo)
        {
            Random800_90 rand = new Random800_90();
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, min, max, inc));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithMessageLength(messageLength)
                    .WithOutputLength(new MathDomain().AddSegment(new RangeDomainSegment(null, 256, 512, 8)))
                    .Build()
            );

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase(4, 65537, 1, "SHAKE-128")]
        [TestCase(-1, 65536, 1, "SHAKE-128")]
        [TestCase(7, 65537, 1, "SHAKE-256")]
        [TestCase(-1, 65536, 1, "SHAKE-256")]
        public void ShouldReturnErrorWithInvalidOutputLength(int min, int max, int inc, string algo)
        {
            Random800_90 rand = new Random800_90();
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, 16, 65536, 8));
            
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithRevision("2.0")
                    .WithMessageLength(messageLength)
                    .WithOutputLength(new MathDomain().AddSegment(new RangeDomainSegment(null, min, max, inc)))
                    .Build()
            );

            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }
        
        [Test]
        [TestCase(16, 65536, 8, "SHAKE-128")]
        [TestCase(224, 65536, 1, "SHAKE-128")]
        [TestCase(32, 65535, 1, "SHAKE-256")]
        [TestCase(16, 1536, 8, "SHAKE-256")]
        public void ShouldReturnWithValidOutputLength(int min, int max, int inc, string algo)
        {
            Random800_90 rand = new Random800_90();
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, 16, 65536, 8));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algo)
                    .WithMessageLength(messageLength)
                    .WithOutputLength(new MathDomain().AddSegment(new RangeDomainSegment(null, min, max, inc)))
                    .Build()
            );

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }
        
        public class ParameterBuilder
        {
            private string _algorithm;
            private string _revision;
            public List<string> _digests;
            private MathDomain _messageLength;
            private MathDomain _outputLength;
            private readonly Random800_90 _rand = new Random800_90();

            public ParameterBuilder()
            {
                _algorithm = "SHAKE-128";
                _revision = "FIPS202";
                _digests = new List<string> { "128" };

                _messageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 65536));
                _outputLength = new MathDomain().AddSegment(new RangeDomainSegment(_rand, 256, 512, 8));
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
            
            public ParameterBuilder WithOutputLength(MathDomain value)
            {
                _outputLength = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Revision = _revision,
                    MessageLength = _messageLength,
                    OutputLength = _outputLength
                };
            }
        }
    }
}

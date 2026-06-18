using System.Collections.Generic;
using NIST.CVP.ACVTS.Libraries.Generation.BLAKE2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.BLAKE2
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldAcceptValidBlake2bParameters()
        {
            var subject = new ParameterValidator();

            var result = subject.Validate(new ParameterBuilder().Build());

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase("BLAKE2s")]
        [TestCase("BLAKE2")]
        public void ShouldRejectUnsupportedAlgorithms(string algorithm)
        {
            var subject = new ParameterValidator();

            var result = subject.Validate(new ParameterBuilder().WithAlgorithm(algorithm).Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(0)]
        [TestCase(7)]
        [TestCase(520)]
        public void ShouldRejectInvalidDigestLengths(int digestLength)
        {
            var subject = new ParameterValidator();

            var result = subject.Validate(new ParameterBuilder().WithDigestLengths(digestLength).Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldRejectMessageLengthsThatAreNotByteAligned()
        {
            var subject = new ParameterValidator();

            var result = subject.Validate(new ParameterBuilder()
                .WithMessageLength(new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 31, 1)))
                .Build());

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldRejectKeyLengthsThatAreTooLarge()
        {
            var subject = new ParameterValidator();

            var result = subject.Validate(new ParameterBuilder()
                .WithKeyLength(new MathDomain().AddSegment(new ValueDomainSegment(520)))
                .Build());

            Assert.That(result.Success, Is.False);
        }

        public class ParameterBuilder
        {
            private string _algorithm = "BLAKE2b";
            private List<int> _digestLengths = new List<int> { 256, 512 };
            private MathDomain _messageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 1024, 8));
            private MathDomain _keyLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 0, 512, 8));

            public ParameterBuilder WithAlgorithm(string algorithm)
            {
                _algorithm = algorithm;
                return this;
            }

            public ParameterBuilder WithDigestLengths(params int[] digestLengths)
            {
                _digestLengths = new List<int>(digestLengths);
                return this;
            }

            public ParameterBuilder WithMessageLength(MathDomain messageLength)
            {
                _messageLength = messageLength;
                return this;
            }

            public ParameterBuilder WithKeyLength(MathDomain keyLength)
            {
                _keyLength = keyLength;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Revision = "1.0",
                    DigestLengths = _digestLengths,
                    MessageLength = _messageLength,
                    KeyLength = _keyLength
                };
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.cSHAKE.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.cSHAKE
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
        [TestCase("Invalid valid", "notValid")]
        [TestCase("Partially valid", "CSHAK")]
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
        [TestCase(16, 65537, true)]
        [TestCase(65535, 65535, false)]
        [TestCase(15, 16, true)]
        [TestCase(17, 50091, false)]
        public void ShouldReturnErrorWithInvalidOutputLength(int min, int max, bool bitOriented)
        {
            var outputLength = new MathDomain();
            outputLength.AddSegment(new ValueDomainSegment(min));
            outputLength.AddSegment(new ValueDomainSegment(max));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("cshake")
                    .WithOutputLength(outputLength)
                    .Build()
            );

            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        [TestCase(16, 65537, true)]
        [TestCase(65535, 65535, false)]
        [TestCase(15, 16, true)]
        [TestCase(17, 50091, false)]
        public void ShouldReturnErrorWithInvalidMessageLength(int min, int max, bool bitOriented)
        {
            var messageLength = new MathDomain();
            messageLength.AddSegment(new ValueDomainSegment(min));
            messageLength.AddSegment(new ValueDomainSegment(max));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("cshake")
                    .WithOutputLength(messageLength)
                    .Build()
            );

            Assert.That(result.Success, Is.False, result.ErrorMessage);
        }

        [Test]
        [TestCase(127)]
        [TestCase(512)]
        // int cannot be null [TestCase(null)]
        public void ShouldRejectBadcSHAKEDigestSize(int number)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("cshake")
                    .WithDigestSizes(new int[] { number })
                    .Build()
            );

            Assert.That(result.Success, Is.False);
        }

        [Test]
        [TestCase(16, 17, true)]
        [TestCase(65528, 65536, false)]
        [TestCase(256, 512, false)]
        [TestCase(17, 50091, true)]
        public void ShouldReturnSuccessWithNewOutputLength(int min, int max, bool bitOriented)
        {
            var outputLength = new MathDomain();
            outputLength.AddSegment(new RangeDomainSegment(null, min, max, bitOriented ? 1 : 8));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 })
                    .WithOutputLength(outputLength)
                    .Build()
            );

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        [Test]
        [TestCase(16, 17, true)]
        [TestCase(65528, 65536, false)]
        [TestCase(256, 512, false)]
        [TestCase(17, 50091, true)]
        public void ShouldReturnSuccessWithNewMessageLength(int min, int max, bool bitOriented)
        {
            var messageLength = new MathDomain();
            messageLength.AddSegment(new RangeDomainSegment(null, min, max, bitOriented ? 1 : 8));

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { 128 })
                    .WithMessageLength(messageLength)
                    .Build()
            );

            Assert.That(result.Success, Is.True, result.ErrorMessage);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private List<int> _digestSize;
            private bool _hexCustomization;
            private MathDomain _outputLength;
            private MathDomain _messageLength;

            public ParameterBuilder()
            {
                _algorithm = "cSHAKE-128";
                _digestSize = new List<int>() { 128 };
                _hexCustomization = false;
                _outputLength = new MathDomain();
                _outputLength.AddSegment(new RangeDomainSegment(null, 16, 65536));
                _messageLength = new MathDomain();
                _messageLength.AddSegment(new RangeDomainSegment(null, 16, 65536));
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(int[] values)
            {
                _digestSize = values.ToList();
                return this;
            }

            public ParameterBuilder WithHexCustomization(bool value)
            {
                _hexCustomization = value;
                return this;
            }

            public ParameterBuilder WithOutputLength(MathDomain value)
            {
                _outputLength = value;
                return this;
            }

            public ParameterBuilder WithMessageLength(MathDomain value)
            {
                _messageLength = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    DigestSizes = _digestSize.ToList(),
                    HexCustomization = _hexCustomization,
                    OutputLength = _outputLength,
                    MessageLength = _messageLength
                };
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.SHA2.v1_0;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA2
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        [Test]
        public void ShouldReturnNoErrorsWithValidParameters()
        {
            var subject = new ParameterValidator();
            var parameterBuilder = new ParameterBuilder();
            var parameters = parameterBuilder.Build();
            var result = subject.Validate(parameters);

            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase("Invalid valid", "notValid")]
        [TestCase("Partially valid", "SHA")]
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
        [TestCase("null", new object[] { null })]
        [TestCase("Invalid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "512t256", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "512t256", null })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var strDigs = digestSize.Select(v => (string)v).ToList();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA2-512/256")
                    .WithDigestSizes(strDigs)
                    .Build()
            );

            Assert.That(result.Success, Is.False, label);
        }

        [Test]
        public void ShouldRejectBadSHA1DigestSize()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA1")
                    .WithDigestSizes(new List<string>() { "256" })
                    .Build()
            );

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldRejectBadSHA2DigestSize()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA2")
                    .WithDigestSizes(new List<string>() { "160" })
                    .Build()
            );

            Assert.That(result.Success, Is.False);
        }

        [Test]
        public void ShouldReturnSuccessWithNewMessageLength()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMessageLength(
                        new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)))
                    .Build()
            );

            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase(0, 1024)]
        [TestCase(1720, 12144)]
        public void ShouldReturnSuccessForValidMessageLengths(int min, int max)
        {
          var subject = new ParameterValidator();
          var result = subject.Validate(
            new ParameterBuilder()
              .WithMessageLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), min, max, 8)))
              .Build()
          );

            Assert.That(result.Success, Is.True);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private List<string> _digestSizes;
            private MathDomain _messageLength;

            public ParameterBuilder()
            {
                _algorithm = "SHA2-224";
                _digestSizes = new List<string>() { "224", "256" };
                _messageLength = new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 0, 65535));
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(List<string> value)
            {
                _digestSizes = value;
                return this;
            }

            public ParameterBuilder WithMessageLength(MathDomain messageLength)
            {
                _messageLength = messageLength;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Revision = "1.0",
                    DigestSizes = _digestSizes,
                    MessageLength = _messageLength
                };
            }
        }
    }
}

using System.Collections.Generic;
using NIST.CVP.Generation.SHA2.v1_0;
using NIST.CVP.Math;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;
using System.Linq;

namespace NIST.CVP.Generation.SHA2.Tests
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

            Assert.IsFalse(result.Success, label);
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

            Assert.IsFalse(result.Success, label);
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

            Assert.IsFalse(result.Success);
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

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewMessageLength()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMessageLength(new MathDomain().AddSegment(new RangeDomainSegment(new Random800_90(), 8, 1024, 8)))
                    .Build()
            );

            Assert.IsTrue(result.Success);
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

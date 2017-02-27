using System.Linq;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA2.Tests
{
    [TestFixture]
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
        [TestCase("Invalid valid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "SHA1", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "SHA2", null })]
        public void ShouldReturnErrorWithInvalidMode(string label, object[] mode)
        {
            var strModes = mode.Select(v => (string) v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithMode(strModes)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        [Test]
        [TestCase("null", new object[] { null })]
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid valid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "224", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "512t256", null })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var strDigs = digestSize.Select(v => (string)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestSize(strDigs)
                    .Build()
            );

            Assert.IsFalse(result.Success, label);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string[] _mode;
            private string[] _digestSize;

            public ParameterBuilder()
            {
                _algorithm = "SHA";
                _mode = ParameterValidator.VALID_MODES;
                _digestSize = ParameterValidator.VALID_DIGEST_SIZES;
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithMode(string[] value)
            {
                _mode = value;
                return this;
            }

            public ParameterBuilder WithDigestSize(string[] value)
            {
                _digestSize = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    DigestSize = _digestSize,
                    Mode = _mode
                };
            }
        }
    }
}

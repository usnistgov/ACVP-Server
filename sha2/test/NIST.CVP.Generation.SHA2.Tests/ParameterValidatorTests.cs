using System.Linq;
using Castle.Core.Internal;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

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
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "224", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "512t256", null })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var strDigs = digestSize.Select(v => (string)v).ToArray();
            
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA2")
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
                    .WithDigestSizes(new [] {"256"})
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
                    .WithDigestSizes(new [] {"160"})
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewIncludeNull()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithIncludeNull("no")
                    .Build()
            );
            
            Assert.IsTrue(result.Success);
        }

        [Test]
        public void ShouldReturnSuccessWithNewBitOriented()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithBitOriented("no")
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private string[] _digestSizes;
            private string _includeNull;
            private string _bitOriented;

            public ParameterBuilder()
            {
                _algorithm = "SHA2";
                _digestSizes = new[] {"224", "256"};
                _includeNull = "yes";
                _bitOriented = "yes";
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(string[] value)
            {
                _digestSizes = value;
                return this;
            }

            public ParameterBuilder WithIncludeNull(string value)
            {
                _includeNull = value;
                return this;
            }

            public ParameterBuilder WithBitOriented(string value)
            {
                _bitOriented = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    DigestSizes = _digestSizes,
                    BitOriented = _bitOriented,
                    IncludeNull = _includeNull
                };
            }
        }
    }
}

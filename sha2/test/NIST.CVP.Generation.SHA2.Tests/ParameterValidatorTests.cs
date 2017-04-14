using System.Linq;
using Castle.Core.Internal;
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
        [TestCase("Invalid valid", new object[] { "notValid" })]
        [TestCase("Partially valid", new object[] { "SHA1", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "SHA1", null })]
        public void ShouldReturnErrorWithInvalidMode(string label, object[] mode)
        {
            var strModes = mode.Select(v => (string) v).ToArray();
            var functions = new Function[strModes.Length];

            for (var i = 0; i < strModes.Length; i++)
            {
                functions[i] = new Function
                {
                    Mode = strModes[i],
                    DigestSizes = new [] {"224"}
                };
            }
          
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFunctions(functions)
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
            var functions = new []
            {
                new Function
                {
                    Mode = "sha2",
                    DigestSizes = strDigs
                }
            };
            
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFunctions(functions)
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
                    .WithFunctions(
                        new Function[]
                        {
                            new Function
                            {
                                Mode = "sha1",
                                DigestSizes = new [] {"224"}
                            }
                        })
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
                    .WithFunctions(
                        new Function[]
                        {
                            new Function
                            {
                                Mode = "sha2",
                                DigestSizes = new [] {"160"}
                            }
                        })
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
                    .WithIncludeNull(false)
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
                    .WithBitOriented(false)
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private Function[] _functions;
            private bool _includeNull;
            private bool _bitOriented;

            public ParameterBuilder()
            {
                _algorithm = "SHA";
                _functions = new[]
                {
                    new Function
                    {
                        Mode = "sha1",
                        DigestSizes = new[] {"160"}
                    },
                    new Function
                    {
                        Mode = "sha2",
                        DigestSizes = new [] {"224", "256", "384", "512", "512/224", "512/256"}
                    }
                };
                _includeNull = true;
                _bitOriented = true;
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithFunctions(Function[] value)
            {
                _functions = value;
                return this;
            }

            public ParameterBuilder WithIncludeNull(bool value)
            {
                _includeNull = value;
                return this;
            }

            public ParameterBuilder WithBitOriented(bool value)
            {
                _bitOriented = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Functions = _functions,
                    BitOriented = _bitOriented,
                    IncludeNull = _includeNull
                };
            }
        }
    }
}

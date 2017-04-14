using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
        [TestCase("Partially valid", new object[] { "SHA3", "notValid" })]
        [TestCase("Partially valid with null", new object[] { "SHAKE", null })]
        public void ShouldReturnErrorWithInvalidMode(string label, object[] mode)
        {
            var strModes = mode.Select(v => (string)v).ToArray();
            var functions = new Function[strModes.Length];

            for (var i = 0; i < strModes.Length; i++)
            {
                functions[i] = new Function
                {
                    Mode = strModes[i],
                    DigestSizes = new[] { 224 }
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
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid valid", new object[] { 152 })]
        [TestCase("Partially valid", new object[] { 224, 9000 })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var strDigs = digestSize.Select(v => (int)v).ToArray();
            var functions = new[]
            {
                new Function
                {
                    Mode = "sha3",
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
        public void ShouldRejectBadSHA3DigestSize()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFunctions(
                        new Function[]
                        {
                            new Function
                            {
                                Mode = "sha3",
                                DigestSizes = new [] {128}
                            }
                        })
                    .Build()
            );

            Assert.IsFalse(result.Success);
        }

        [Test]
        public void ShouldRejectBadSHAKEDigestSize()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithFunctions(
                        new Function[]
                        {
                            new Function
                            {
                                Mode = "shake",
                                DigestSizes = new[] {224}
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
        public void ShouldReturnSuccessWithNewBitOrientedInput()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithBitOrientedInput(false)
                    .Build()
            );

            Assert.IsTrue(result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private Function[] _functions;
            private bool _includeNull;
            private bool _bitOrientedInput;

            public ParameterBuilder()
            {
                _algorithm = "SHA3";
                _functions = new[]
                {
                    new Function
                    {
                        Mode = "sha3",
                        DigestSizes = new[] {224, 256, 384, 512}
                    },
                    new Function
                    {
                        Mode = "shake",
                        DigestSizes = new [] {128, 256}
                    }
                };
                _includeNull = true;
                _bitOrientedInput = true;
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

            public ParameterBuilder WithBitOrientedInput(bool value)
            {
                _bitOrientedInput = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    Functions = _functions,
                    BitOrientedInput = _bitOrientedInput,
                    BitOrientedOutput = false,
                    IncludeNull = _includeNull,
                    MinOutputLength = 16,
                    MaxOutputLength = 65536
                };
            }
        }
    }
}

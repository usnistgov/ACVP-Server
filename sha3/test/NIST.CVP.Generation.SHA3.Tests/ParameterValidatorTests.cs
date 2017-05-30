using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.SHA3.Tests
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
        [TestCase("empty", new object[] { })]
        [TestCase("Invalid", new object[] { 225 })]
        [TestCase("Partially valid", new object[] { 224, 200 })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var intDigs = digestSize.Select(v => (int)v).ToArray();

            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA3")
                    .WithDigestSizes(intDigs)
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
                    .WithAlgorithm("SHA3")
                    .WithDigestSizes(new[] { 128 })
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
                    .WithAlgorithm("SHAKE")
                    .WithDigestSizes(new[] { 224 })
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
            private int[] _digestSizes;
            private bool _includeNull;
            private bool _bitOrientedInput;

            public ParameterBuilder()
            {
                _algorithm = "SHA3";
                _digestSizes = new [] { 224 };
                _includeNull = true;
                _bitOrientedInput = true;
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(int[] value)
            {
                _digestSizes = value;
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
                    DigestSizes = _digestSizes,
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

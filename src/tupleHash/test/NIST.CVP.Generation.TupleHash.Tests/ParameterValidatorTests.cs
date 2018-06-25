using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.TupleHash.Tests
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
        [TestCase("Partially valid", "TupleHas")]
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
                    .WithAlgorithm("tuplehash")
                    .WithOutputLength(outputLength)
                    .WithBitOrientedOutput(bitOriented)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(127)]
        [TestCase(512)]
        [TestCase(null)]
        public void ShouldRejectBadCSHAKEDigestSize(int number)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("tuplehash")
                    .WithDigestSizes(new int[]{ number })
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
                    .WithAlgorithm("tuplehash")
                    .WithDigestSizes(new int[] { 128 })
                    .WithOutputLength(outputLength)
                    .WithBitOrientedOutput(bitOriented)
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private int[] _digestSize;
            private bool _includeNull;
            private bool _bitOrientedInput;
            private bool _bitOrientedOutput;
            private MathDomain _outputLength;
            private bool _xof;

            public ParameterBuilder()
            {
                _algorithm = "tuplehash";
                _digestSize = new int[] { 128, 256 };
                _includeNull = true;
                _bitOrientedInput = true;
                _bitOrientedOutput = true;
                _outputLength = new MathDomain();
                _outputLength.AddSegment(new RangeDomainSegment(null, 16, 65536));
                _xof = true;
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(int[] values)
            {
                _digestSize = values;
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

            public ParameterBuilder WithBitOrientedOutput(bool value)
            {
                _bitOrientedOutput = value;
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
                    DigestSizes = _digestSize,
                    BitOrientedInput = _bitOrientedInput,
                    BitOrientedOutput = _bitOrientedOutput,
                    IncludeNull = _includeNull,
                    OutputLength = _outputLength,
                    XOF = _xof
                };
            }
        }
    }
}

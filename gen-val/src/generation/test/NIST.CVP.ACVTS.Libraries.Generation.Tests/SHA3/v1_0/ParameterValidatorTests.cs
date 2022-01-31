using System.Collections.Generic;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.SHA3.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.SHA3.v1_0
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
        [TestCase("Invalid", new object[] { 225 })]
        [TestCase("Partially valid", new object[] { 224, 200 })]
        public void ShouldReturnErrorWithInvalidDigestSize(string label, object[] digestSize)
        {
            var intDigs = digestSize.Select(v => (int)v).ToList();

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
                    .WithAlgorithm("shake")
                    .WithDigestSizes(new List<int>() { 128 })
                    .WithOutputLength(outputLength)
                    .WithBitOrientedOutput(bitOriented)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        [Test]
        public void ShouldRejectBadSHA3DigestSize()
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm("SHA3")
                    .WithDigestSizes(new List<int>() { 128 })
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
                    .WithDigestSizes(new List<int>() { 224 })
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
        [TestCase("SHAKE-128", 128)]
        [TestCase("SHAKE-256", 256)]
        public void ShouldFailWithoutOutputLengthForShake(string algorithm, int digestSize)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithAlgorithm(algorithm)
                    .WithDigestSizes(new List<int>() { digestSize })
                    .WithOutputLength(null)
                    .Build()
            );

            Assert.IsFalse(result.Success);
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
                    .WithAlgorithm("SHAKE-128")
                    .WithDigestSizes(new List<int>() { 128 })
                    .WithOutputLength(outputLength)
                    .WithBitOrientedOutput(bitOriented)
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private List<int> _digestSizes;
            private bool _includeNull;
            private bool _bitOrientedInput;
            private bool _bitOrientedOutput;
            private MathDomain _outputLength;

            public ParameterBuilder()
            {
                _algorithm = "SHA3-224";
                _digestSizes = new List<int>() { 224 };
                _includeNull = true;
                _bitOrientedInput = true;
                _bitOrientedOutput = true;

                _outputLength = new MathDomain();
                _outputLength.AddSegment(new RangeDomainSegment(null, 16, 65536));
            }

            public ParameterBuilder WithAlgorithm(string value)
            {
                _algorithm = value;
                return this;
            }

            public ParameterBuilder WithDigestSizes(List<int> value)
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
                    DigestSizes = _digestSizes,
                    BitOrientedInput = _bitOrientedInput,
                    BitOrientedOutput = _bitOrientedOutput,
                    IncludeNull = _includeNull,
                    OutputLength = _outputLength
                };
            }
        }
    }
}

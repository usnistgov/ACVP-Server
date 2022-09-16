using System.Linq;
using NIST.CVP.ACVTS.Libraries.Generation.ParallelHash.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.ParallelHash
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
        [TestCase("Partially valid", "ParallelHas")]
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
                    .WithOutputLength(outputLength)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
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
                    .WithMessageLength(messageLength)
                    .Build()
            );

            Assert.IsFalse(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(127)]
        [TestCase(512)]
        [TestCase(null)]
        public void ShouldRejectBadcSHAKEDigestSize(int number)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithDigestSizes(new int[] { number })
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
                    .WithDigestSizes(new int[] { 128 })
                    .WithOutputLength(outputLength)
                    .Build()
            );

            Assert.IsTrue(result.Success, result.ErrorMessage);
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

            Assert.IsTrue(result.Success, result.ErrorMessage);
        }

        [Test]
        [TestCase(new[] { true }, true)]
        [TestCase(new[] { false }, true)]
        [TestCase(new[] { true, false }, true)]
        [TestCase(new[] { false, true }, true)]
        [TestCase(new[] { true, true }, false)]
        [TestCase(new[] { false, false }, false)]
        public void ShouldSucceedOnValidXOFSettingsLen(bool[] XOF, bool isSuccessExpected)
        {
            var subject = new ParameterValidator();
            var result = subject.Validate(
                new ParameterBuilder()
                    .WithXOF(XOF)
                    .Build()
            );

            Assert.AreEqual(isSuccessExpected, result.Success);
        }

        public class ParameterBuilder
        {
            private string _algorithm;
            private int[] _digestSize;
            private MathDomain _outputLength;
            private MathDomain _messageLength;
            private MathDomain _blockSize;
            private bool[] _xof;
            private bool _hexCustomization;

            public ParameterBuilder()
            {
                _algorithm = "ParallelHash-128";
                _digestSize = new int[] { 128, 256 };
                _messageLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 16, 65536));
                _outputLength = new MathDomain().AddSegment(new RangeDomainSegment(null, 16, 65536));
                _blockSize = new MathDomain().AddSegment(new RangeDomainSegment(null, 1, 16));
                _xof = new[] { true };
                _hexCustomization = false;
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

            public ParameterBuilder WithXOF(bool[] value)
            {
                _xof = value;
                return this;
            }

            public ParameterBuilder WithHexCustomization(bool value)
            {
                _hexCustomization = value;
                return this;
            }

            public Parameters Build()
            {
                return new Parameters
                {
                    Algorithm = _algorithm,
                    DigestSizes = _digestSize.ToList(),
                    MessageLength = _messageLength,
                    OutputLength = _outputLength,
                    BlockSize = _blockSize,
                    XOF = _xof,
                    HexCustomization = _hexCustomization
                };
            }
        }
    }
}

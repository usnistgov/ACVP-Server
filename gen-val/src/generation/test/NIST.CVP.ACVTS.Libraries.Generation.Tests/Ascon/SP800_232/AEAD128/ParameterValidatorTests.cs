using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.AEAD128;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.AEAD128;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    [Test]
    [TestCase(BlockCipherDirections.Encrypt, true)]
    [TestCase(BlockCipherDirections.Decrypt, true)]
    [TestCase(BlockCipherDirections.None, false)]
    public void ShouldReturnSuccessWithValidDirections(BlockCipherDirections dir, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithDirection(new[] { dir })
                    .Build()
            );

        Assert.That(expectedSuccess, Is.EqualTo(result.Success), result.ErrorMessage);
    }

    [Test]
    [TestCase(0, 1, true)]
    [TestCase(65535, 65536, true)]
    [TestCase(-1, 0, false)]
    [TestCase(65536, 65537, false)]
    public void ShouldReturnSuccessWithValidPlaintextLengths(int min, int max, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        MathDomain lens = new MathDomain();
        lens.AddSegment(new RangeDomainSegment(null, min, max));
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithPlaintextLength(lens)
                    .Build()
            );

        Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
    }

    [Test]
    [TestCase(0, 1, true)]
    [TestCase(65535, 65536, true)]
    [TestCase(-1, 0, false)]
    [TestCase(65536, 65537, false)]
    public void ShouldReturnSuccessWithValidADLengths(int min, int max, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        MathDomain lens = new MathDomain();
        lens.AddSegment(new RangeDomainSegment(null, min, max));
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithADLength(lens)
                    .Build()
            );

        Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
    }

    [Test]
    [TestCase(64, 65, true)]
    [TestCase(127, 128, true)]
    [TestCase(63, 64, false)]
    [TestCase(128, 129, false)]
    public void ShouldReturnSuccessWithValidTagLengths(int min, int max, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        MathDomain lens = new MathDomain();
        lens.AddSegment(new RangeDomainSegment(null, min, max));
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithTruncationLength(lens)
                    .Build()
            );

        Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
    }

    [Test]
    [TestCase(true, true)]
    [TestCase(false, true)]

    public void ShouldReturnSuccessWithValidNonceMasking(bool masking, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithNonceMasking(new[] { masking })
                    .Build()
            );

        Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private BlockCipherDirections[] _direction;
        private MathDomain _plaintextLength;
        private MathDomain _adLength;
        private MathDomain _tagLength;
        private bool[] _nonceMasking;

        public ParameterBuilder()
        {
            _algorithm = "Ascon-AEAD128";
            _direction = new[] { BlockCipherDirections.Encrypt, BlockCipherDirections.Decrypt };
            _plaintextLength = new MathDomain();
            _plaintextLength.AddSegment(new RangeDomainSegment(null, 0, 65536));
            _adLength = new MathDomain();
            _adLength.AddSegment(new RangeDomainSegment(null, 0, 65536));
            _tagLength = new MathDomain();
            _tagLength.AddSegment(new RangeDomainSegment(null, 64, 128));
            _nonceMasking = new[] {true, false};
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithDirection(BlockCipherDirections[] values)
        {
            _direction = values;
            return this;
        }

        public ParameterBuilder WithPlaintextLength(MathDomain value)
        {
            _plaintextLength = value;
            return this;
        }

        public ParameterBuilder WithADLength(MathDomain value)
        {
            _adLength = value;
            return this;
        }

        public ParameterBuilder WithTruncationLength(MathDomain value)
        {
            _tagLength = value;
            return this;
        }

        public ParameterBuilder WithNonceMasking(bool[] value)
        {
            _nonceMasking = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                Algorithm = _algorithm,
                Directions = _direction,
                PayloadLength = _plaintextLength,
                ADLength = _adLength,
                TagLength = _tagLength,
                SupportsNonceMasking = _nonceMasking
            };
        }
    }
}

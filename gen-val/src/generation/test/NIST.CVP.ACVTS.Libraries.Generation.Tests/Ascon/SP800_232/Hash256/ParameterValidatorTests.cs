using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NIST.CVP.ACVTS.Libraries.Generation.Ascon.SP800_232.Hash256;
using NUnit.Framework;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.Ascon.SP800_232.Hash256;

[TestFixture, UnitTest]
public class ParameterValidatorTests
{
    [Test]
    [TestCase(0, 1, true)]
    [TestCase(65535, 65536, true)]
    [TestCase(-1, 0, false)]
    [TestCase(65536, 65537, false)]
    public void ShouldReturnSuccessWithValidMessageLengths(int min, int max, bool expectedSuccess)
    {
        var validator = new ParameterValidator();
        MathDomain lens = new MathDomain();
        lens.AddSegment(new RangeDomainSegment(null, min, max));
        var result = validator.Validate(
                new ParameterBuilder()
                    .WithMessageLength(lens)
                    .Build()
            );

        Assert.That(result.Success, Is.EqualTo(expectedSuccess), result.ErrorMessage);
    }

    public class ParameterBuilder
    {
        private string _algorithm;
        private MathDomain _messageLength;

        public ParameterBuilder()
        {
            _algorithm = "Ascon-Hash256";
            _messageLength = new MathDomain();
            _messageLength.AddSegment(new RangeDomainSegment(null, 0, 65536));
        }

        public ParameterBuilder WithAlgorithm(string value)
        {
            _algorithm = value;
            return this;
        }

        public ParameterBuilder WithMessageLength(MathDomain value)
        {
            _messageLength = value;
            return this;
        }

        public Parameters Build()
        {
            return new Parameters
            {
                MessageLength = _messageLength,
            };
        }
    }
}

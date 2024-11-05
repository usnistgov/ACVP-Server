using NIST.CVP.ACVTS.Libraries.Generation.KMAC.v1_0;
using NIST.CVP.ACVTS.Libraries.Math.Domain;
using NIST.CVP.ACVTS.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.ACVTS.Libraries.Generation.Tests.KMAC
{
    [TestFixture, UnitTest]
    public class ParameterValidatorTests
    {
        private ParameterValidator _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new ParameterValidator();
        }

        [Test]
        [TestCase("KMAC-128", 256, 256, 256)]
        public void ShouldValidateSuccessfully(string algorithm, int keyLen, int macLen, int msgLen)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(keyLen)))
                .WithMacLen(new MathDomain().AddSegment(new ValueDomainSegment(macLen)))
                .WithMsgLen(new MathDomain().AddSegment(new ValueDomainSegment(msgLen)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.That(result.Success, Is.True);
        }

        [Test]
        [TestCase("test0 - below minimum", 0, 2048, false, false)]
        [TestCase("test1 - at minimum", ParameterValidator._MIN_KEY_LENGTH, 2048, true, false)]
        [TestCase("test2 - not mod 8", 255, 2047, false, false)]
        [TestCase("test3 - at max", 2048, ParameterValidator._MAX_KEY_LENGTH, true, false)]
        [TestCase("test4 - bitOriented", 255, 2049, true, true)]
        public void ShouldSucceedOnValidKeyLen(string label, int keyLenMin, int keyLenMax, bool isSuccessExpected, bool bitOriented)
        {
            Parameters parameters = new ParameterBuilder()
                .WithKeyLen(new MathDomain().AddSegment(new RangeDomainSegment(null, keyLenMin, keyLenMax, bitOriented ? 1 : 8)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.That(result.Success, Is.EqualTo(isSuccessExpected));
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", 64, 2048, true, false)]
        [TestCase("test1 - valid", "KMAC-128", 128, 256, true, false)]
        [TestCase("test2 - invalid", "KMAC-128", 31, 256, false, true)]
        [TestCase("test3 - invalid", "KMAC-128", 256, 65537, false, true)]
        [TestCase("test4 - invalid", "KMAC-128", 255, 2047, false, false)]
        [TestCase("test5 - valid", "KMAC-128", 255, 2049, true, true)]
        public void ShouldSucceedOnValidMacLen(string label, string algorithm, int macLenMin, int macLenMax, bool isSuccessExpected, bool bitOriented)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMacLen(new MathDomain().AddSegment(new RangeDomainSegment(null, macLenMin, macLenMax, bitOriented ? 1 : 8)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.That(result.Success, Is.EqualTo(isSuccessExpected));
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", 64, 2048, true, false)]
        [TestCase("test1 - valid", "KMAC-128", 128, 256, true, false)]
        [TestCase("test2 - invalid", "KMAC-128", -1, 256, false, true)]
        [TestCase("test3 - invalid", "KMAC-128", 256, 65537, false, true)]
        [TestCase("test4 - invalid", "KMAC-128", 255, 2047, false, false)]
        [TestCase("test5 - valid", "KMAC-128", 255, 2049, true, true)]
        public void ShouldSucceedOnValidMsgLen(string label, string algorithm, int msgLenMin, int msgLenMax, bool isSuccessExpected, bool bitOriented)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMsgLen(new MathDomain().AddSegment(new RangeDomainSegment(null, msgLenMin, msgLenMax, bitOriented ? 1 : 8)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.That(result.Success, Is.EqualTo(isSuccessExpected));
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", new[] { true, false }, true)]
        [TestCase("test1 - valid", "KMAC-128", new[] { false, true }, true)]
        [TestCase("test2 - valid", "KMAC-128", new[] { true }, true)]
        [TestCase("test3 - valid", "KMAC-128", new[] { false }, true)]
        [TestCase("test4 - invalid", "KMAC-128", new bool[] { }, false)]
        public void ShouldSucceedOnValidXOFSettingsLen(string label, string algorithm, bool[] XOF, bool isSuccessExpected)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithXOF(XOF)
                .Build();

            var result = _subject.Validate(parameters);

            Assert.That(result.Success, Is.EqualTo(isSuccessExpected));
        }
    }
}

using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.KMAC.Tests
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
        [TestCase("KMAC-256", 256, 256, 256)]
        public void ShouldValidateSuccessfully(string algorithm, int keyLen, int macLen, int msgLen)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(keyLen)))
                .WithMacLen(new MathDomain().AddSegment(new ValueDomainSegment(macLen)))
                .WithMsgLen(new MathDomain().AddSegment(new ValueDomainSegment(msgLen)))
                .Build();
            
            var result = _subject.Validate(parameters);
            
            Assert.IsTrue(result.Success);
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
            
            Assert.AreEqual(isSuccessExpected, result.Success);
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", 64, 2048, true, false)]
        [TestCase("test1 - valid", "KMAC-128", 128, 256, true, false)]
        [TestCase("test2 - invalid", "KMAC-128", 31, 256, false, true)]
        [TestCase("test3 - invalid", "KMAC-128", 256, 65537, false, true)]
        [TestCase("test4 - invalid", "KMAC-128", 255, 2047, false, false)]
        [TestCase("test5 - valid", "KMAC-128", 255, 2049, true, true)]

        [TestCase("test6 - valid", "KMAC-256", 64, 2048, true, false)]
        [TestCase("test7 - valid", "KMAC-256", 128, 256, true, false)]
        [TestCase("test8 - invalid", "KMAC-256", 31, 256, false, true)]
        [TestCase("test9 - invalid", "KMAC-256", 256, 65537, false, true)]
        [TestCase("test10 - invalid", "KMAC-256", 255, 2047, false, false)]
        [TestCase("test11 - valid", "KMAC-256", 255, 2049, true, true)]
        public void ShouldSucceedOnValidMacLen(string label, string algorithm, int macLenMin, int macLenMax, bool isSuccessExpected, bool bitOriented)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMacLen(new MathDomain().AddSegment(new RangeDomainSegment(null, macLenMin, macLenMax, bitOriented ? 1 : 8)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.AreEqual(isSuccessExpected, result.Success);
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", 64, 2048, true, false)]
        [TestCase("test1 - valid", "KMAC-128", 128, 256, true, false)]
        [TestCase("test2 - invalid", "KMAC-128", -1, 256, false, true)]
        [TestCase("test3 - invalid", "KMAC-128", 256, 65537, false, true)]
        [TestCase("test4 - invalid", "KMAC-128", 255, 2047, false, false)]
        [TestCase("test5 - valid", "KMAC-128", 255, 2049, true, true)]

        [TestCase("test6 - valid", "KMAC-256", 64, 2048, true, false)]
        [TestCase("test7 - valid", "KMAC-256", 128, 256, true, false)]
        [TestCase("test8 - invalid", "KMAC-256", -1, 256, false, true)]
        [TestCase("test9 - invalid", "KMAC-256", 256, 65537, false, true)]
        [TestCase("test10 - invalid", "KMAC-256", 255, 2047, false, false)]
        [TestCase("test11 - valid", "KMAC-256", 255, 2049, true, true)]
        public void ShouldSucceedOnValidMsgLen(string label, string algorithm, int msgLenMin, int msgLenMax, bool isSuccessExpected, bool bitOriented)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMsgLen(new MathDomain().AddSegment(new RangeDomainSegment(null, msgLenMin, msgLenMax, bitOriented ? 1 : 8)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.AreEqual(isSuccessExpected, result.Success);
        }

        [Test]
        [TestCase("test0 - valid", "KMAC-128", true, false, true)]
        [TestCase("test1 - valid", "KMAC-128", false, true, true)]
        [TestCase("test2 - valid", "KMAC-128", true, true, true)]
        [TestCase("test3 - invalid", "KMAC-128", false, false, false)]

        [TestCase("test4 - valid", "KMAC-256", true, false, true)]
        [TestCase("test5 - valid", "KMAC-256", false, true, true)]
        [TestCase("test6 - valid", "KMAC-256", true, true, true)]
        [TestCase("test7 - invalid", "KMAC-256", false, false, false)]
        public void ShouldSucceedOnValidXOFSettingsLen(string label, string algorithm, bool nonXOF, bool XOF, bool isSuccessExpected)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithNonXOF(nonXOF)
                .WithXOF(XOF)
                .Build();

            var result = _subject.Validate(parameters);

            Assert.AreEqual(isSuccessExpected, result.Success);
        }
    }
}
using NIST.CVP.Math.Domain;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.HMAC.Tests
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
        [TestCase("HMAC-SHA-1", 160, 160)]
        [TestCase("HMAC-SHA2-224", 224, 224)]
        [TestCase("HMAC-SHA2-256", 256, 256)]
        [TestCase("HMAC-SHA2-384", 384, 384)]
        [TestCase("HMAC-SHA2-512", 512, 512)]
        [TestCase("HMAC-SHA2-512/224", 224, 224)]
        [TestCase("HMAC-SHA2-512/256", 256, 256)]
        [TestCase("HMAC-SHA3-224", 224, 224)]
        [TestCase("HMAC-SHA3-256", 256, 256)]
        [TestCase("HMAC-SHA3-384", 384, 384)]
        [TestCase("HMAC-SHA3-512", 512, 512)]
        public void ShouldValidateSuccessfully(string algorithm, int keyLen, int macLen)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(keyLen)))
                .WithMacLen(new MathDomain().AddSegment(new ValueDomainSegment(macLen)))
                .Build();
            
            var result = _subject.Validate(parameters);
            
            Assert.IsTrue(result.Success);
        }

        [Test]
        [TestCase("test0 - below minimum", 0, false)]
        [TestCase("test1 - at minimum", ParameterValidator._MIN_KEY_LENGTH, true)]
        [TestCase("test2 - not mod 8", 15, false)]
        [TestCase("test3 - at max", ParameterValidator._MAX_KEY_LENGTH, true)]
        public void ShouldSucceedOnValidKeyLen(string label, int keyLen, bool isSuccessExpected)
        {
            // SHA flavor does not matter for keylen
            Parameters parameters = new ParameterBuilder()
                .WithKeyLen(new MathDomain().AddSegment(new ValueDomainSegment(keyLen)))
                .Build();

            var result = _subject.Validate(parameters);
            
            Assert.AreEqual(isSuccessExpected, result.Success);
        }

        [Test]
        [TestCase("test0 - sha1 valid", "HMAC-SHA-1", 80, true)]
        [TestCase("test1 - sha1 invalid", "HMAC-SHA-1", 79, false)]
        [TestCase("test2 - sha1 invalid", "HMAC-SHA-1", 161, false)]
        [TestCase("test3 - sha2-224 valid", "HMAC-SHA2-224", 112, true)]
        [TestCase("test4 - sha2-224 invalid", "HMAC-SHA2-224", 111, false)]
        [TestCase("test5 - sha2-224 invalid", "HMAC-SHA2-224", 225, false)]
        [TestCase("test6 - sha2-256 valid", "HMAC-SHA2-256", 128, true)]
        [TestCase("test7 - sha2-256 invalid", "HMAC-SHA2-256", 127, false)]
        [TestCase("test8 - sha2-256 invalid", "HMAC-SHA2-256", 257, false)]
        [TestCase("test9 - sha2-384 valid", "HMAC-SHA2-384", 192, true)]
        [TestCase("test10 - sha2-384 invalid", "HMAC-SHA2-384", 191, false)]
        [TestCase("test11 - sha2-384 invalid", "HMAC-SHA2-384", 385, false)]
        [TestCase("test12 - sha2-512 valid", "HMAC-SHA2-512", 256, true)]
        [TestCase("test13 - sha2-512 invalid", "HMAC-SHA2-512", 254, false)]
        [TestCase("test14 - sha2-512 invalid", "HMAC-SHA2-512", 513, false)]
        [TestCase("test15 - sha2-512/224 valid", "HMAC-SHA2-512/224", 112, true)]
        [TestCase("test16 - sha2-512/224 invalid", "HMAC-SHA2-512/224", 111, false)]
        [TestCase("test17 - sha2-512/224 invalid", "HMAC-SHA2-512/224", 225, false)]
        [TestCase("test18 - sha2-512/256 valid", "HMAC-SHA2-512/256", 128, true)]
        [TestCase("test19 - sha2-512/256 invalid", "HMAC-SHA2-512/256", 127, false)]
        [TestCase("test20 - sha2-512/256 invalid", "HMAC-SHA2-512/256", 257, false)]
        [TestCase("test21 - sha3-224 valid", "HMAC-SHA3-224", 112, true)]
        [TestCase("test22 - sha3-224 invalid", "HMAC-SHA3-224", 111, false)]
        [TestCase("test23 - sha3-224 invalid", "HMAC-SHA3-224", 225, false)]
        [TestCase("test24 - sha3-256 valid", "HMAC-SHA3-256", 128, true)]
        [TestCase("test25 - sha3-256 invalid", "HMAC-SHA3-256", 127, false)]
        [TestCase("test26 - sha3-256 invalid", "HMAC-SHA3-256", 257, false)]
        [TestCase("test27 - sha3-384 valid", "HMAC-SHA3-384", 192, true)]
        [TestCase("test28 - sha3-384 invalid", "HMAC-SHA3-384", 191, false)]
        [TestCase("test29 - sha3-384 invalid", "HMAC-SHA3-384", 385, false)]
        [TestCase("test30 - sha3-512 valid", "HMAC-SHA3-512", 256, true)]
        [TestCase("test31 - sha3-512 invalid", "HMAC-SHA3-512", 254, false)]
        [TestCase("test32 - sha3-512 invalid", "HMAC-SHA3-512", 513, false)]
        public void ShouldSucceedOnValidMacLen(string label, string algorithm, int macLen, bool isSuccessExpected)
        {
            Parameters parameters = new ParameterBuilder()
                .WithAlgorithm(algorithm)
                .WithMacLen(new MathDomain().AddSegment(new ValueDomainSegment(macLen)))
                .Build();

            var result = _subject.Validate(parameters);

            Assert.AreEqual(isSuccessExpected, result.Success);
        }
    }
}
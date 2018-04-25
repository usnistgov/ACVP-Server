using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        [Test]
        [TestCase("not relevant", "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "singleblock", typeof(TestCaseGeneratorSingleBlockEncrypt))]
        [TestCase("Encrypt", "sInGlEbLoCk", typeof(TestCaseGeneratorSingleBlockEncrypt))]
        [TestCase("DEcrypt", "SINGLEBLOCK", typeof(TestCaseGeneratorSingleBlockDecrypt))]
        [TestCase("Decrypt", "SingleBlock", typeof(TestCaseGeneratorSingleBlockDecrypt))]
        [TestCase("encrypt", "PartialBlock", typeof(TestCaseGeneratorPartialBlockEncrypt))]
        [TestCase("ENCRYPT", "PARTIALBLOCK", typeof(TestCaseGeneratorPartialBlockEncrypt))]
        [TestCase("deCRYPT", "partialBLOCK", typeof(TestCaseGeneratorPartialBlockDecrypt))]
        [TestCase("Decrypt", "PaRtIaLbLoCk", typeof(TestCaseGeneratorPartialBlockDecrypt))]
        [TestCase("EncRypt", "counter", typeof(TestCaseGeneratorCounterEncrypt))]
        [TestCase("ENCRYPT", "COUNTER", typeof(TestCaseGeneratorCounterEncrypt))]
        [TestCase("decrypt", "Counter", typeof(TestCaseGeneratorCounterDecrypt))]
        [TestCase("Decrypt", "COUNTer", typeof(TestCaseGeneratorCounterDecrypt))]
        [TestCase("Junk", "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Direction = direction,
                TestType = testType,
                KeyLength = 128
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var testGroup = new TestGroup
            {
                Direction = string.Empty,
                TestType = string.Empty
            };

            var subject = new TestCaseGeneratorFactory(null, null);
            var generator = subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using NIST.CVP.Crypto.Common.Symmetric.Engines;
using NIST.CVP.Crypto.Common.Symmetric.Enums;
using NIST.CVP.Tests.Core.TestCategoryAttributes;
using NUnit.Framework;

namespace NIST.CVP.Generation.AES_CTR.Tests
{
    [TestFixture, UnitTest]
    public class TestCaseGeneratorFactoryTests
    {
        private TestCaseGeneratorFactory _subject;

        [SetUp]
        public void Setup()
        {
            _subject = new TestCaseGeneratorFactory(null);
        }

        [Test]
        [TestCase("not relevant", "gfsbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "keysbox", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "vartxt", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("not relevant", "varkey", typeof(TestCaseGeneratorKnownAnswer))]
        [TestCase("encrypt", "singleblock", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("Encrypt", "sInGlEbLoCk", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("DEcrypt", "SINGLEBLOCK", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("Decrypt", "SingleBlock", typeof(TestCaseGeneratorSingleBlock))]
        [TestCase("encrypt", "PartialBlock", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("ENCRYPT", "PARTIALBLOCK", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("deCRYPT", "partialBLOCK", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("Decrypt", "PaRtIaLbLoCk", typeof(TestCaseGeneratorPartialBlock))]
        [TestCase("EncRypt", "ctr", typeof(TestCaseGeneratorCounter))]
        [TestCase("ENCRYPT", "CTR", typeof(TestCaseGeneratorCounter))]
        [TestCase("decrypt", "CtR", typeof(TestCaseGeneratorCounter))]
        [TestCase("Decrypt", "cTr", typeof(TestCaseGeneratorCounter))]
        [TestCase("Junk", "Junk", typeof(TestCaseGeneratorNull))]
        [TestCase("", "", typeof(TestCaseGeneratorNull))]
        public void ShouldReturnProperGenerator(string direction, string testType, Type expectedType)
        {
            var testGroup = new TestGroup
            {
                Direction = direction,
                InternalTestType = testType,
                KeyLength = 128
            };

            _subject = new TestCaseGeneratorFactory(null);
            var generator = _subject.GetCaseGenerator(testGroup);
            Assume.That(generator != null);
            Assert.IsInstanceOf(expectedType, generator);
        }

        [Test]
        public void ShouldReturnAGenerator()
        {
            var testGroup = new TestGroup
            {
                Direction = string.Empty,
                InternalTestType = string.Empty
            };

            _subject = new TestCaseGeneratorFactory(null);
            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}

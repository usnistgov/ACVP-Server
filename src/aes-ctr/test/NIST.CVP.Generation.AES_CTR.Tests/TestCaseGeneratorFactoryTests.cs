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
        private Mock<IBlockCipherEngine> _engine;
        private Mock<IBlockCipherEngineFactory> _engineFactory;

        [SetUp]
        public void Setup()
        {
            _engine = new Mock<IBlockCipherEngine>();
            _engineFactory = new Mock<IBlockCipherEngineFactory>();
            _engineFactory
                .Setup(s => s.GetSymmetricCipherPrimitive(It.IsAny<BlockCipherEngines>()))
                .Returns(_engine.Object);
            _subject = new TestCaseGeneratorFactory(null, _engineFactory.Object, null, null);
        }

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

            _subject = new TestCaseGeneratorFactory(null, _engineFactory.Object, null, null);
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
                TestType = string.Empty
            };

            _subject = new TestCaseGeneratorFactory(null, _engineFactory.Object, null, null);
            var generator = _subject.GetCaseGenerator(testGroup);
            Assert.IsNotNull(generator);
        }
    }
}
